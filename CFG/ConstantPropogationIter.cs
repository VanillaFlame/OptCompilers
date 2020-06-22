using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.CFG
{
    public enum SemilatticeData { UNDEF = 0, CONST = 1, NAC = 2 }

    public struct SemilatticeValue
    {
        public SemilatticeData Type { get; private set; }
        public string ConstValue { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var temp = (SemilatticeValue)obj;
            return Type == temp.Type && ConstValue == temp.ConstValue;
        }

        public static bool operator ==(SemilatticeValue first, SemilatticeValue second) =>
            first.Equals(second);

        public static bool operator !=(SemilatticeValue first, SemilatticeValue second) =>
            !first.Equals(second);

        public SemilatticeValue(SemilatticeData type, string str = null)
        {
            Type = type;
            ConstValue = str;
        }

        public SemilatticeValue(SemilatticeData type, int val)
        {
            Type = type;
            ConstValue = val.ToString();
        }

        public override int GetHashCode()
            => (Type.ToString() + ConstValue).GetHashCode();

        public SemilatticeValue collecting(SemilatticeValue second) =>
            Type == SemilatticeData.NAC || second.Type == SemilatticeData.NAC
            ? new SemilatticeValue(SemilatticeData.NAC)
            : Type == SemilatticeData.UNDEF
            ? second
            : second.Type == SemilatticeData.UNDEF
            ? this
            : ConstValue == second.ConstValue
            ? second
            : new SemilatticeValue(SemilatticeData.NAC);

        public override string ToString()
            => Type.ToString() + " " + ConstValue;
    }

    public class INsOUTs
    {
        public Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>> IN { get; set; }
        public Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>> OUT { get; set; }

        public INsOUTs()
        {
            IN = new Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>>();
            OUT = new Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>>();
        }
    }

    public class ConstantPropogationIter : IterAlgoGeneric<Dictionary<string, SemilatticeValue>>
    {
       

        public HashSet<string> untreatedTypes = new HashSet<string>() {
            "OR",
            "AND",
            "EQUAL",
            "NOTEQUAL",
            "GREATERTHAN",
            "LESSTHAN",
            "GREATEROREQUAL",
            "LESSOREQUAL",
            "NOT"
        };

        public int FindOperations(int v1, int v2, string op)
        {
            switch (op)
            {
                case "PLUS":
                    return v1 + v2;
                case "MULT":
                    return v1 * v2;
                case "DIVIDE":
                    return v1 / v2;
                case "MULTIPLICATE":
                    return v1 - v2;
            }
            return 0;
        }

        public Dictionary<string, SemilatticeValue> Transfer(BasicBlock basicBlock, Dictionary<string, SemilatticeValue> IN)
        {
            var OUT = IN.ToDictionary(entry => entry.Key, entry => entry.Value);
            var instrs = basicBlock.Instructions;
            for (var i = 0; i < instrs.Count; i++)
            {
                if (instrs[i].Result.StartsWith("#"))
                {
                    OUT.Add(instrs[i].Result, new SemilatticeValue(SemilatticeData.UNDEF));

                    string first, second, operation;

                    first = instrs[i].Argument1;
                    second = instrs[i].Argument2;
                    operation = instrs[i].Operation;

                    if (first == "True" || second == "True" || second == "False" || second == "False" || untreatedTypes.Contains(operation))
                    {
                        OUT[instrs[i].Result] = new SemilatticeValue(SemilatticeData.NAC);
                    }
                    else if (int.TryParse(first, out var v2) && OUT[second].Type == SemilatticeData.CONST)
                    {
                        int.TryParse(OUT[second].ConstValue, out var val2);
                        OUT[instrs[i].Result] = new SemilatticeValue(SemilatticeData.CONST, FindOperations(val2, v2, operation).ToString());
                    }
                    else if (OUT[first].Type == SemilatticeData.CONST && int.TryParse(second, out var v1))
                    {
                        int.TryParse(OUT[first].ConstValue, out var val1);
                        OUT[instrs[i].Result] = new SemilatticeValue(SemilatticeData.CONST, FindOperations(val1, v1, operation).ToString());
                    }
                    else if (OUT[first].Type == SemilatticeData.CONST && OUT[second].Type == SemilatticeData.CONST)
                    {
                        int.TryParse(OUT[first].ConstValue, out var val1);
                        int.TryParse(OUT[second].ConstValue, out var val2);
                        OUT[instrs[i].Result] = new SemilatticeValue(SemilatticeData.CONST, FindOperations(val1, val2, operation).ToString());
                    }
                    else
                    {
                        OUT[instrs[i].Result] =
                            OUT[first].Type == SemilatticeData.UNDEF
                            ? new SemilatticeValue(SemilatticeData.UNDEF)
                            : OUT[first].Type == SemilatticeData.NAC || OUT[second].Type == SemilatticeData.NAC
                            ? new SemilatticeValue(SemilatticeData.NAC)
                            : new SemilatticeValue(SemilatticeData.UNDEF);
                    }
                }

                if (instrs[i].Operation == "=")
                {
                    if (int.TryParse(instrs[i].Argument1, out var s))
                    {
                        OUT[instrs[i].Result] = new SemilatticeValue(SemilatticeData.CONST, s);
                    }
                    else
                    {
                        var operation = instrs[i].Operation;
                        var first = instrs[i].Argument1;

                        OUT[instrs[i].Result] =
                            untreatedTypes.Contains(operation)
                            ? new SemilatticeValue(SemilatticeData.NAC)
                            : first == "True" || first == "False"
                            ? new SemilatticeValue(SemilatticeData.NAC)
                            : OUT[first].Type == SemilatticeData.CONST
                            ? new SemilatticeValue(SemilatticeData.CONST, OUT[first].ConstValue)
                            : OUT[first].Type == SemilatticeData.NAC
                            ? new SemilatticeValue(SemilatticeData.NAC)
                            : new SemilatticeValue(SemilatticeData.UNDEF);
                    }
                }
            }
            var temp_keys = OUT.Keys.Where(x => x.StartsWith("#")).ToList();
            foreach (var k in temp_keys)
            {
                OUT.Remove(k);
            }

            return OUT;
        }

        public INsOUTs ExecuteNonGeneric(ControlFlowGraph g)
        {
            var blocks = g.blocks;
            var INs = new Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>>();
            var OUTs = new Dictionary<BasicBlock, Dictionary<string, SemilatticeValue>>();

            var variables = new HashSet<string>();

            foreach (var block in blocks)
            {
                foreach (var instr in block.Instructions)
                {
                    if (instr.Result != "" && !instr.Result.StartsWith("#") && !instr.Result.StartsWith("L") && !variables.Contains(instr.Result))
                        variables.Add(instr.Result);

                    if (instr.Argument1 != "" && !instr.Argument1.StartsWith("#") && !instr.Argument1.StartsWith("L") && instr.Argument1 != "True"
                        && instr.Argument1 != "False" && !int.TryParse(instr.Argument1, out var temp1) && !variables.Contains(instr.Argument1))
                        variables.Add(instr.Argument1);

                    if (instr.Argument2 != "" && !instr.Argument2.StartsWith("#") && !instr.Argument2.StartsWith("L") && instr.Argument2 != "True" && instr.Argument2 != "False"
                        && !int.TryParse(instr.Argument2, out var temp2) && !variables.Contains(instr.Argument2))
                        variables.Add(instr.Argument2);
                }
            }
            var temp = new Dictionary<string, SemilatticeValue>();
            foreach (var elem in variables)
            {
                temp.Add(elem, new SemilatticeValue(SemilatticeData.UNDEF));
            }

            foreach (var block in blocks)
            {
                INs.Add(block, temp.ToDictionary(entry => entry.Key, entry => entry.Value));
                OUTs.Add(block, temp.ToDictionary(entry => entry.Key, entry => entry.Value));
            }

            var Changed = true;
            while (Changed)
            {
                Changed = false;
                foreach (var block in blocks)
                {
                    var parents = g._parents[block.Index].Select(z => z.block);
                    INs[block] = parents.Select(x => OUTs[x])
                        .Aggregate(temp.ToDictionary(entry => entry.Key, entry => entry.Value), (x, y) => Collect(x, y));
                    var newOut = Transfer(block, INs[block]);
                    if (OUTs[block].Where(entry => newOut[entry.Key] != entry.Value).Any())
                    {
                        Changed = true;
                        OUTs[block] = newOut;
                    }
                }
            }

            return new INsOUTs { IN = INs, OUT = OUTs };
        }

        public override directed directed => directed.forward;

        public override Func<Dictionary<string, SemilatticeValue>, Dictionary<string, SemilatticeValue>, bool> Compare
            => (a, b) => !a.Where(entry => b[entry.Key] != entry.Value).Any();

        public static Dictionary<string, SemilatticeValue> Collect(Dictionary<string, SemilatticeValue> first, Dictionary<string, SemilatticeValue> second)
        {
            var result = new Dictionary<string, SemilatticeValue>(first.Count, first.Comparer);
            foreach (var elem in second)
            {
                result[elem.Key] = first[elem.Key].collecting(elem.Value);
            }

            return result;
        }

        public override
            Func<Dictionary<string, SemilatticeValue>, Dictionary<string, SemilatticeValue>, Dictionary<string, SemilatticeValue>>
            CollectingOperator => (a, b) => Collect(a, b);

        public override Func<BasicBlock, Dictionary<string, SemilatticeValue>, Dictionary<string, SemilatticeValue>>
            TransferFunction
        { get; protected set; }

        public override Dictionary<string, SemilatticeValue> Init { get; protected set; }

        public override InOutData<Dictionary<string, SemilatticeValue>> Execute(ControlFlowGraph graph)
        {
            var blocks = graph.blocks;
            var variables = new HashSet<string>();
            foreach (var block in blocks)
            {
                foreach (var instr in block.Instructions)
                {
                    if (instr.Result != "" && !instr.Result.StartsWith("#") && !instr.Result.StartsWith("L") && !variables.Contains(instr.Result))
                    {
                        variables.Add(instr.Result);
                    }

                    if (instr.Argument1 != "" && !instr.Argument1.StartsWith("#") && !instr.Argument1.StartsWith("L") && instr.Argument1 != "True"
                        && instr.Argument1 != "False" && !int.TryParse(instr.Argument1, out var temp1) && !variables.Contains(instr.Argument1))
                    {
                        variables.Add(instr.Argument1);
                    }

                    if (instr.Argument2 != "" && !instr.Argument2.StartsWith("#") && !instr.Argument2.StartsWith("L") && instr.Argument2 != "True" && instr.Argument2 != "False"
                        && !int.TryParse(instr.Argument2, out var temp2) && !variables.Contains(instr.Argument2))
                    {
                        variables.Add(instr.Argument2);
                    }
                }
            }
            var temp = new Dictionary<string, SemilatticeValue>();
            foreach (var elem in variables)
            {
                temp.Add(elem, new SemilatticeValue(SemilatticeData.UNDEF));
            }

            Init = temp;
            TransferFunction = Transfer;
            return base.Execute(graph);
        }
    }
}