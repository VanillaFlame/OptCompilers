using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class PrettyPrinterVisitor : Visitor
    {
        public string FormattedProgram 
        {
            get
            {
                return ProgramBuilder.ToString();
            }
        }

        private StringBuilder ProgramBuilder = new StringBuilder();
        private int Indent = 0;
        private int IndentDelta = 2;

        private void AddSpace()
        {
            ProgramBuilder.Append(' ');
        }

        private void NewLine()
        {
            ProgramBuilder.Append('\n');
        }

        private void WriteIndent()
        {
            for (int i = 0; i < Indent; ++i)
            {
                AddSpace();
            }
        }

        private void IncreaseIndent()
        {
            Indent += IndentDelta;
        }

        private void DecreaseIndent()
        {
            Indent -= IndentDelta;
        }

        public override void Visit(IdNode node)
        {
            ProgramBuilder.Append(node.Name);
        }
        public override void Visit(IntNumNode node)
        {
            ProgramBuilder.Append(node.Num);
        }
        public override void Visit(FloatNumNode node)
        {
            ProgramBuilder.Append(node.Num);
        }
        public override void Visit(BoolValNode node)
        {
            ProgramBuilder.Append(node.Val);
        }
        public override void Visit(BinExprNode node)
        {
            node.Left.Visit(this);
            AddSpace();
            ProgramBuilder.Append(BinOpTypeExtensions.ToFriendlyString(node.OpType));
            AddSpace();
            node.Right.Visit(this);
        }
        public override void Visit(UnoExprNode node)
        {
            ProgramBuilder.Append(UnoOpTypeExtensions.ToFriendlyString(node.OpType));
            node.Expr.Visit(this);
        }
        public override void Visit(AssignNode node)
        {
            WriteIndent();
            node.Id.Visit(this);
            AddSpace();
            ProgramBuilder.Append(AssignTypeExtensions.ToFriendlyString(node.AssOp));
            AddSpace();
            node.Expr.Visit(this);
            ProgramBuilder.Append(";");
            NewLine();
        }
        public override void Visit(ForNode node)
        {
            WriteIndent();
            ProgramBuilder.Append("for");
            AddSpace();
            node.Counter.Visit(this);
            ProgramBuilder.Append(" = ");
            node.Begin.Visit(this);
            ProgramBuilder.Append("..");
            node.End.Visit(this);
            NewLine();
            node.Stat.Visit(this);
        }
        public override void Visit(WhileNode node)
        {
            WriteIndent();
            ProgramBuilder.Append("while (");
            node.Condition.Visit(this);
            ProgramBuilder.Append(")");
            NewLine();
            node.Stat.Visit(this);
            NewLine();
        }
        public override void Visit(GotoNode node)
        {
            WriteIndent();
            ProgramBuilder.Append("goto ");
            ProgramBuilder.Append(node.Label);
            ProgramBuilder.Append(";");
            NewLine();
        }
        public override void Visit(LabeledStatementNode node)
        {
            WriteIndent();
            ProgramBuilder.Append(node.Label);
            ProgramBuilder.Append(": ");
            node.Stat.Visit(this);
        }
        public override void Visit(IfNode node)
        {
            WriteIndent();
            ProgramBuilder.Append("if ");
            node.Condition.Visit(this);
            NewLine();
            node.Stat.Visit(this);
            if (node.ElseStat != null)
            {
                ProgramBuilder.Append("else");
                NewLine();
                node.ElseStat.Visit(this);
            }
        }
        public override void Visit(WriteNode node)
        {
            WriteIndent();
            ProgramBuilder.Append(node.WriteLine ? "writeln" : "write");
            ProgramBuilder.Append('(');
            node.ExprList.Visit(this);
            ProgramBuilder.Append(')');
            NewLine();
        }
        public override void Visit(ReadNode node)
        {
            WriteIndent();
            ProgramBuilder.Append("read");
            ProgramBuilder.Append('(');
            node.Ident.Visit(this);
            ProgramBuilder.Append(')');
            NewLine();
        }
        public override void Visit(BlockNode node)
        {
            WriteIndent();
            ProgramBuilder.Append('{');
            NewLine();
            IncreaseIndent();
            foreach (var s in node.StList)
            {
                s.Visit(this);
            }
            DecreaseIndent();
            WriteIndent();
            ProgramBuilder.Append('}');
            NewLine();
        }
        public override void Visit(DeclarationNode node)
        {
            WriteIndent();
            ProgramBuilder.Append(node.Type.ToString().ToLower());
            AddSpace();
            for (int i = 0; i < node.IdentList.Count - 1; ++i)
            {
                node.IdentList[i].Visit(this);
                ProgramBuilder.Append(", ");
            }
            node.IdentList[node.IdentList.Count - 1].Visit(this);
            ProgramBuilder.Append(';');
            NewLine();
        }
        public override void Visit(ListExprNode node)
        {
            for (int i = 0; i < node.ExprList.Count - 1; ++i)
            {
                node.ExprList[i].Visit(this);
                ProgramBuilder.Append(", ");
            }
            node.ExprList[node.ExprList.Count - 1].Visit(this);
        }
    }
}
