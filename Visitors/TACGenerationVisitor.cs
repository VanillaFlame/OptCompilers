using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class TACGenerationVisitor : AutoVisitor
    {
        public List<TACCommand> TACCommands = new List<TACCommand>();

        private static string TEMP_NAME_PREFIX = "#t";
        private static string TEMP_LABEL_PREFIX = "#L";
        public override void Visit(AssignNode node)
        {
            var tmp = GenerateExprTAC(node.Expr);
            AddCommand(node.AssOp.ToFriendlyString(), tmp, "", node.Id.Name);
        }

        public override void Visit(IfNode node)
        {
            var genCond = GenerateExprTAC(node.Condition);
            var label1 = GenerateTempLabel();
            var label2 = GenerateTempLabel();
            AddCommand("if goto", genCond, label1, "");
            node.ElseStat.Visit(this);
            AddCommand("goto", label2, "", "");
            AddCommand("", "", "", "", label1);
            node.Stat.Visit(this);
            AddCommand("", "", "", "", label2);
        }

        public override void Visit(WhileNode node)
        {
            var label1 = GenerateTempLabel();
            var label2 = GenerateTempLabel();
            var label3 = GenerateTempLabel();
            AddCommand("", "", "", "", label1);
            var genCond = GenerateExprTAC(node.Condition);
            AddCommand("if goto", genCond, label2, "");
            AddCommand("goto", label3, "", "");
            AddCommand("", "", "", "", label2);
            node.Stat.Visit(this);
            AddCommand("goto", label1, "", "");
            AddCommand("", "", "", "", label3);
        }

        private string GenerateExprTAC(ExprNode ex)
        {
            if (ex.GetType() == typeof(BinExprNode))
            {
                var bin = ex as BinExprNode;
                string tmp1 = GenerateExprTAC(bin.Left);
                string tmp2 = GenerateExprTAC(bin.Right);
                string tmp = GenerateTempName();
                AddCommand(bin.OpType.ToFriendlyString(), tmp1, tmp2, tmp);
                return tmp;
            }
            if (ex.GetType() == typeof(UnoExprNode))
            {
                var uno = ex as UnoExprNode;
                string tmp1 = GenerateExprTAC(uno.Expr);
                string tmp = GenerateTempName();
                AddCommand(uno.OpType.ToFriendlyString(), tmp1, "", tmp);
                return tmp;
            }
            if (ex is IdNode id)
            {
                return id.Name;
            }
            if (ex is FloatNumNode f)
            {
                return f.Num.ToString();
            }
            if (ex is BoolValNode b)
            {
                return b.Val.ToString();
            }
            if (ex is IntNumNode i)
            {
                return i.Num.ToString();
            }
            return "";
        }

        private void AddCommand(string operation, string arg1, string arg2, string result, string label = "")
        {
            TACCommands.Add(new TACCommand(operation, arg1, arg2, result, label));
        }

        private int tempVariableCounter = 0;
        private string GenerateTempName()
        {
            return TEMP_NAME_PREFIX + (tempVariableCounter++);
        }

        private int tempLabelCounter = 0;
        private string GenerateTempLabel()
        {
            return TEMP_LABEL_PREFIX + (tempLabelCounter++);
        }
    }
}
