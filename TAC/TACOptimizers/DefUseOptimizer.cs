using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLang.TAC;
using SimpleLang.TACOptimizers;

namespace SimpleLang.TACOptimizers
{
    public class DefUseOptimizer : TACOptimizer
    {
        public static List<Def> DefList;

        internal static bool IsId(string id) =>
            id != null && id != "" && id != "True" && id != "False" &&
                    (char.IsLetter(id[0]) || id[0] == '#') && !(id is "#L");

        private static void AddUse(string id, TACInstruction c, int num)
        {
            if (IsId(id))
            {
                var def = DefList.FindLastIndex(x => x.Id == id);
                var use = new Use(num, c);

                if (def != -1)
                {
                    use.Parent = DefList[def];
                    DefList[def].Uses.Add(use);
                }
            }
        }

        static List<string> operations = new List<string>()
           { "+", "-", "*", "/", "==", "!=", "<", "<=", ">", ">=", "&", "|", "!", "="};

        public DefUseOptimizer(ThreeAddressCode tac) : base(tac)
        {
        }

        private static void FillLists(List<TACInstruction> commands)
        {
            DefList = new List<Def>();
            for (int i = 0; i < commands.Count; ++i)
            {
                if (operations.Contains(commands[i].Operation))
                    DefList.Add(new Def(i, commands[i].Result));
                AddUse(commands[i].Argument1, commands[i], i);
                AddUse(commands[i].Argument2, commands[i], i);
            }
        }

        private static void DeleteUse(string id, int i)
        {
            if (id == "" || id == null) return;
            var d = DefList.FindLast(x => x.Id == id && x.OrderNum < i);
            if (d == null) return;
            d.Uses.RemoveAt(d.Uses.FindLastIndex(x => x.OrderNum == i));
        }

        public static Tuple<bool, List<TACInstruction>> DeleteDeadCode(List<TACInstruction> commands)
        {
            List<TACInstruction> result = new List<TACInstruction>();
            FillLists(commands);
            bool isChange = false;

            for (int i = commands.Count - 1; i >= 0; --i)
            {
                var c = commands[i];
                var lastDefInd = DefList.FindLastIndex(x => x.Id == c.Result);
                var curDefInd = DefList.FindIndex(x => x.OrderNum == i);

                if (curDefInd != -1 && DefList[curDefInd].Uses.Count == 0
                        && (c.Result[0] != '#' ? curDefInd != lastDefInd : true))
                {
                    DeleteUse(commands[i].Argument1, i);
                    DeleteUse(commands[i].Argument2, i);
                    result.Add(new TACInstruction("", "", "", "", commands[i].Label));
                    isChange = true;
                }
                else result.Add(commands[i]);
            }
            result.Reverse();
            return Tuple.Create(isChange, result);
        }

        public override void Run()
        {
            var res = DeleteDeadCode(Instructions);
            if (res.Item1)
            {
                Instructions = res.Item2;
                TAC = new ThreeAddressCode(Instructions);
            }
        }
    }

    public class Use
    {
        public Def Parent { get; set; }
        public int OrderNum { get; set; }
        public TACInstruction Command { get; set; }

        public Use(int n, TACInstruction c, Def p = null)
        {
            Parent = p;
            Command = c;
            OrderNum = n;
        }
    }

    public class Def
    {
        public List<Use> Uses { get; set; }
        public int OrderNum { get; set; }
        public string Id { get; set; }

        public Def(int n, string id)
        {
            Uses = new List<Use>();
            Id = id;
            OrderNum = n;
        }
    }
}

