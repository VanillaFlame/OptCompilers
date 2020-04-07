using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class AutoVisitor : Visitor
    {
        public override void Visit(BinExprNode node)
        {
            node.Left.Visit(this);
            node.Right.Visit(this);
        }
        public override void Visit(UnoExprNode node)
        {
            node.Expr.Visit(this);
        }
        public override void Visit(AssignNode node)
        {
            node.Id.Visit(this);
            node.Expr.Visit(this);
        }
        public override void Visit(ForNode node)
        {
            node.Counter.Visit(this);
            node.Begin.Visit(this);
            node.End.Visit(this);
            node.Stat.Visit(this);
        }
        public override void Visit(WhileNode node)
        {
            node.Condition.Visit(this);
            node.Stat.Visit(this);
        }
        public override void Visit(LabeledStatementNode node)
        {
            node.Stat.Visit(this);
        }
        public override void Visit(IfNode node)
        {
            node.Condition.Visit(this);
            node.Stat.Visit(this);
            node.ElseStat?.Visit(this);
        }
        public override void Visit(WriteNode node)
        {
            node.ExprList.Visit(this);
        }
        public override void Visit(ReadNode node)
        {
            node.Ident.Visit(this);
        }
        public override void Visit(BlockNode node)
        { 
            for (int i = node.StList.Count - 1; i >= 0; --i)
            {
                node.StList[i].Visit(this);
            }
        }
        public override void Visit(DeclarationNode node)
        {
            foreach (var i in node.IdentList)
            {
                i.Visit(this);
            }
        }
        public override void Visit(ListExprNode node)
        { 
            foreach (var e in node.ExprList)
            {
                e.Visit(this);
            }
        }
    }
}
