using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class TACGenerationVisitor : AutoVisitor
    {
        public List<TACInstruction> Instructions = new List<TACInstruction>();

        private static string TEMP_NAME_PREFIX = "#t";
        private static string TEMP_LABEL_PREFIX = "#L";

        public override void Visit(AssignNode node)
        {
            var tmp = GenerateTACExpr(node.Expr);
            AddInstruction(node.AssOp.ToFriendlyString(), tmp, "", node.Id.Name);
        }

        public override void Visit(IfNode node)
        {
            var genCond = GenerateTACExpr(node.Condition);
            var label1 = GenerateTempLabel();
            var label2 = GenerateTempLabel();
            AddInstruction("if goto", genCond, label1, "");
            if (node.ElseStat != null)
                node.ElseStat.Visit(this);
            AddInstruction("goto", label2, "", "");
            AddInstruction("", "", "", "", label1);
            node.Stat.Visit(this);
            AddInstruction("", "", "", "", label2);
        }

        public override void Visit(WhileNode node)
        {
            var loopBeginLabel = GenerateTempLabel();
            var loopBodyLabel = GenerateTempLabel();
            var loopEndLabel = GenerateTempLabel();
            AddInstruction("", "", "", "", loopBeginLabel);
            var genCond = GenerateTACExpr(node.Condition);
            AddInstruction("if goto", genCond, loopBodyLabel, "");
            AddInstruction("goto", loopEndLabel, "", "");
            AddInstruction("", "", "", "", loopBodyLabel);
            node.Stat.Visit(this);
            AddInstruction("goto", loopBeginLabel, "", "");
            AddInstruction("", "", "", "", loopEndLabel);
        }

        public override void Visit(GotoNode node)
        {
            AddInstruction("goto", node.Label.ToString(), "", "");
        }

        public override void Visit(LabeledStatementNode node)
        {
            AddInstruction("", "", "", "", node.Label.ToString());
            node.Stat.Visit(this);
        }
        
        public override void Visit(ForNode node)
        {
            var loopBeginLabel = GenerateTempLabel();
            var loopEndLabel = GenerateTempLabel();
            var begin = GenerateTACExpr(node.Begin);
            AddInstruction(AssignType.Assign.ToFriendlyString(), begin, "", node.Counter.Name);
            var end = GenerateTACExpr(node.End);
            var condVariable = GenerateTempName();
            AddInstruction(BinOpType.LessOrEqual.ToFriendlyString(), node.Counter.Name, end, condVariable, loopBeginLabel);
            AddInstruction("if goto", condVariable, loopEndLabel, "");
            node.Stat.Visit(this);
            AddInstruction(BinOpType.Plus.ToFriendlyString(), node.Counter.Name, "1", node.Counter.Name);
            AddInstruction("goto", loopBeginLabel, "", "");
            AddInstruction("", "", "", "", loopEndLabel);
        }

        private string GenerateTACExpr(ExprNode ex)
        {
            if (ex.GetType() == typeof(BinExprNode))
            {
                var bin = ex as BinExprNode;
                string tmp1 = GenerateTACExpr(bin.Left);
                string tmp2 = GenerateTACExpr(bin.Right);
                string tmp = GenerateTempName();
                AddInstruction(bin.OpType.ToFriendlyString(), tmp1, tmp2, tmp);
                return tmp;
            }
            if (ex.GetType() == typeof(UnoExprNode))
            {
                var uno = ex as UnoExprNode;
                string tmp1 = GenerateTACExpr(uno.Expr);
                string tmp = GenerateTempName();
                AddInstruction(uno.OpType.ToFriendlyString(), tmp1, "", tmp);
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

        private void AddInstruction(string operation, string arg1, string arg2, string result, string label = "")
        {
            Instructions.Add(new TACInstruction(operation, arg1, arg2, result, label));
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
