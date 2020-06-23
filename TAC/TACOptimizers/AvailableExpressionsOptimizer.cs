using SimpleLang.CFG;
using SimpleLang.TAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.TACOptimizers
{
    public class AvailableExpressionsOptimizer : TACOptimizer
    {
        private Dictionary<BasicBlock, Gen> gens = new Dictionary<BasicBlock, Gen>();
        private Dictionary<BasicBlock, Kill> kills = new Dictionary<BasicBlock, Kill>();

        public Dictionary<BasicBlock, AvailableExpressionsTable> In = new Dictionary<BasicBlock, AvailableExpressionsTable>();
        public Dictionary<BasicBlock, AvailableExpressionsTable> Out = new Dictionary<BasicBlock, AvailableExpressionsTable>();

        public AvailableExpressionsOptimizer()
        {
        }

        public AvailableExpressionsOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        private void CreateGen(BasicBlock block)
        {
            var gen = new Gen();
            foreach (var instruction in block.Instructions)
            {
                if (instruction.Operation.IsArithmetic())
                {
                    gen.AddNewExpression(instruction);
                }
                if (instruction.Result.IsVariable())
                {
                    gen.RemoveExpression(instruction.Result);
                }
            }
            gens.Add(block, gen);
        }

        private void CreateKill(BasicBlock block)
        {
            var kill = new Kill();
            foreach (var instruction in block.Instructions)
            {
                if (instruction.Operation.IsArithmetic())
                {
                    kill.AddNewExpression(instruction);
                }
                if (instruction.Result.IsVariable())
                {
                    kill.AddKillVariable(instruction.Result);
                }
            }
            kills.Add(block, kill);
        }

        public override void Run()
        {
        gens = new Dictionary<BasicBlock, Gen>();
        kills = new Dictionary<BasicBlock, Kill>();

        In = new Dictionary<BasicBlock, AvailableExpressionsTable>();
        Out = new Dictionary<BasicBlock, AvailableExpressionsTable>();
        Run(this.Cfg, this.Blocks);
        }

        public void Run(ControlFlowGraph cfg, List<BasicBlock> blocks)
        {
            CalculateInOut(cfg, blocks);
            foreach (var block in blocks)
            {
                OptimizeBlock(block);
            }
        }

        private void CalculateInOut(ControlFlowGraph cfg, List<BasicBlock> blocks)
        {
            if (!Out.ContainsKey(cfg.start))
            {
                Out.Add(cfg.start, AvailableExpressionsTable.NewEmpty());
            }
            foreach (var block in blocks)
            {
                CreateGen(block);
                CreateKill(block);
                Out.Add(block, AvailableExpressionsTable.NewUniversal());
            }
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var block in blocks)
                {
                    var prevOuts = new List<AvailableExpressionsTable>();
                    foreach (var prev in block.In)
                    {
                        prevOuts.Add(Out[prev]);
                    }
                    In[block] = AvailableExpressionsTable.Intersection(prevOuts);
                    var oldOut = Out[block];
                    Out[block] = CalculateOut(In[block], block);
                    if (!oldOut.Equals(Out[block]))
                    {
                        changed = true;
                    }
                }
            }
        }

        private void OptimizeBlock(BasicBlock block)
        {
            var currentTable = In[block].Copy();
            for (int i = 0; i < block.Instructions.Count; ++i)
            {
                if (block.Instructions[i].Operation.IsArithmetic())
                {
                    var expression = new AvailableExpression(
                        block.Instructions[i].Argument1,
                        block.Instructions[i].Argument2,
                        block.Instructions[i].Operation);

                    if (currentTable.ExpressionsTable.ContainsKey(expression))
                    {
                        block.Instructions[i].Operation = "=";
                        block.Instructions[i].Argument1 = currentTable.ExpressionsTable[expression];
                        block.Instructions[i].Argument2 = "";
                    } else
                    {
                        currentTable.AddExpression(expression, block.Instructions[i]);
                    }
                }
                    if (block.Instructions[i].Result.IsVariable())
                {
                    currentTable.RemoveExpressions(block.Instructions[i].Result);
                }
            }
        }

        public AvailableExpressionsTable CalculateOut(AvailableExpressionsTable inTable, BasicBlock block)
        {
            var newTable = inTable.Copy();
            var kill = kills[block];
            foreach (var killVariable in kill.KillVariables)
            {
                newTable.RemoveExpressions(killVariable);
            }
            newTable.AddExpressions(kill.GenExpressions, kill.GenInstructions);
            return newTable;
        }

        public class AvailableExpressionsTable
        {
            public class TableElemComparer : EqualityComparer<KeyValuePair<AvailableExpression, string>>
            {
                public override bool Equals(KeyValuePair<AvailableExpression, string> x, KeyValuePair<AvailableExpression, string> y)
                {
                    return x.Key.Equals(y.Key) && x.Value.Equals(y.Value);
                }

                public override int GetHashCode(KeyValuePair<AvailableExpression, string> obj)
                {
                    return (obj.Key.Argument1 + obj.Key.Argument2 + obj.Key.Operation + obj.Value).GetHashCode();
                }
            }

            public Dictionary<AvailableExpression, string> ExpressionsTable = new Dictionary<AvailableExpression, string>();
            
            public bool IsEmpty { get; set; }

            public bool IsUniversal { get; set; }

            public AvailableExpressionsTable() { }
            public AvailableExpressionsTable(Dictionary<AvailableExpression, string> table)
            {
                ExpressionsTable = table;
            }

            public AvailableExpressionsTable Copy()
            {
                var newTable = new Dictionary<AvailableExpression, string>();
                foreach (var key in ExpressionsTable.Keys)
                {
                    newTable.Add(key.Copy(), ExpressionsTable[key]);
                }
                return new AvailableExpressionsTable(newTable);
            }

            public override int GetHashCode()
            {
                return ExpressionsTable.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                var second = obj as AvailableExpressionsTable;
                if (second == null) return false;
                foreach (var k in ExpressionsTable.Keys)
                {
                    if (!second.ExpressionsTable.ContainsKey(k) || !second.ExpressionsTable[k].Equals(ExpressionsTable[k]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void RemoveExpressions(string killVariable)
            {
                var keys = ExpressionsTable.Keys.ToList();
                foreach (var expr in keys)
                {
                    if (expr.UseVariable(killVariable))
                    {
                        ExpressionsTable.Remove(expr);
                    }
                }
            }

            public void AddExpressions(List<AvailableExpression> newExpressions, List<TACInstruction> instrutions)
            {
                for (int i = 0; i < newExpressions.Count; ++i)
                {
                    if (!ExpressionsTable.ContainsKey(newExpressions[i]))
                    {
                        ExpressionsTable.Add(newExpressions[i], instrutions[i].Result);
                    }
                }
            }

            public void AddExpression(AvailableExpression newExpression, TACInstruction instrution)
            {
                if (!ExpressionsTable.ContainsKey(newExpression))
                {
                    ExpressionsTable.Add(newExpression, instrution.Result);
                }
            }

            public static AvailableExpressionsTable NewEmpty()
            {
                var res = new AvailableExpressionsTable();
                res.IsEmpty = true;
                return res;
            }

            public static AvailableExpressionsTable NewUniversal()
            {
                var res = new AvailableExpressionsTable();
                res.IsUniversal = true;
                return res;
            }

            public static AvailableExpressionsTable Intersection(List<AvailableExpressionsTable> tables)
            {
                if (tables.Count == 0)
                {
                    return NewUniversal();
                }

                var j = 0;
                while (tables[j].IsUniversal)
                {
                    ++j;
                }
                if (j == tables.Count)
                {
                    return NewUniversal();
                }

                var res = tables[j].ExpressionsTable;
                for (int i = j + 1; i < tables.Count; ++i)
                {
                    if (tables[i].IsUniversal)
                    {
                        continue;
                    }
                    res = res.Intersect(
                        tables[i].ExpressionsTable,
                        new TableElemComparer())
                        .ToDictionary(x => x.Key, x => x.Value);
                }
                return new AvailableExpressionsTable(res);
            }
        }

        public class AvailableExpression : IComparable
        {
            public string Argument1 { get; set; }
            public string Argument2 { get; set; }
            public string Operation { get; set; }
            public AvailableExpression(string arg1, string arg2, string op)
            {
                Argument1 = arg1;
                Argument2 = arg2;
                Operation = op;
            }
            public bool UseVariable(string variable)
            {
                return Argument1.Equals(variable) || Argument2.Equals(variable);
            }

            public AvailableExpression Copy()
            {
                return new AvailableExpression(Argument1, Argument2, Operation);
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                var second = obj as AvailableExpression;
                if (second == null) return false;
                return Argument1.Equals(second.Argument1) 
                    && Argument2.Equals(second.Argument2) 
                    && Operation.Equals(second.Operation);
            }

            public override int GetHashCode()
            {
                return (Argument1 + Argument2 + Operation).GetHashCode();
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;
                var second = obj as AvailableExpression;
                if (second != null)
                {
                    var arg1Res = Argument1.CompareTo(second.Argument1);
                    if (arg1Res == 0)
                    {
                        var arg2Res = Argument2.CompareTo(second.Argument2);
                        if (arg2Res == 0)
                        {
                            return Operation.CompareTo(second.Operation);
                        }
                        else
                        {
                            return arg2Res;
                        }
                    } else
                    {
                        return arg1Res;
                    }
                } else
                {
                    return 1;
                }
            }
        }

        public class Gen
        {
            public List<TACInstruction> GenInstructions = new List<TACInstruction>();
            public List<AvailableExpression> GenExpressions = new List<AvailableExpression> ();

            public void AddNewExpression(TACInstruction instruction)
            {
                GenInstructions.Add(instruction);
                GenExpressions.Add(new AvailableExpression(instruction.Argument1, instruction.Argument2, instruction.Operation));
            }

            public void RemoveExpression(string usedVariable)
            {
                for (int i = GenExpressions.Count - 1; i >= 0; --i)
                {
                    if (GenExpressions[i].UseVariable(usedVariable))
                    {
                        GenExpressions.RemoveAt(i);
                        GenInstructions.RemoveAt(i);
                    }
                }
            }
        }

        public class Kill
        {
            public HashSet<string> KillVariables = new HashSet<string>();
            public List<TACInstruction> GenInstructions = new List<TACInstruction>();
            public List<AvailableExpression> GenExpressions = new List<AvailableExpression>();

            public void AddKillVariable(string variable)
            {
                KillVariables.Add(variable);
                RemoveExpression(variable);
            }

            public void AddNewExpression(TACInstruction instruction)
            {
                GenInstructions.Add(instruction);
                GenExpressions.Add(new AvailableExpression(instruction.Argument1, instruction.Argument2, instruction.Operation));
            }

            private void RemoveExpression(string usedVariable)
            {
                for (int i = GenExpressions.Count - 1; i >= 0; --i)
                {
                    if (GenExpressions[i].UseVariable(usedVariable))
                    {
                        GenExpressions.RemoveAt(i);
                        GenInstructions.RemoveAt(i);
                    }
                }
            }
        }
    }
}
