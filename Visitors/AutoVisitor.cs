using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class AutoVisitor : Visitor
    {

        public virtual void PostVisit(Node n) { }
        public virtual void PreVisit(Node n) { }
        public override void Visit(BinExprNode node)
        {
            PreVisit(node);
            node.Left.Visit(this);
            node.Right.Visit(this);
            PostVisit(node);
        }
        public override void Visit(UnoExprNode node)
        {
            PreVisit(node);
            node.Expr.Visit(this);
            PostVisit(node);
        }
        public override void Visit(AssignNode node)
        {
            PreVisit(node);
            node.Id.Visit(this);
            node.Expr.Visit(this);
            PostVisit(node);
        }
        public override void Visit(ForNode node)
        {
            PreVisit(node);
            node.Counter.Visit(this);
            node.Begin.Visit(this);
            node.End.Visit(this);
            node.Stat.Visit(this);
            PostVisit(node);
        }
        public override void Visit(WhileNode node)
        {
            PreVisit(node);
            node.Condition.Visit(this);
            node.Stat.Visit(this);
            PostVisit(node);
        }
        public override void Visit(LabeledStatementNode node)
        {
            PreVisit(node);
            node.Stat.Visit(this);
            PostVisit(node);
        }
        public override void Visit(IfNode node)
        {
            PreVisit(node);
            node.Condition.Visit(this);
            node.Stat.Visit(this);
            node.ElseStat?.Visit(this);
            PostVisit(node);
        }
        public override void Visit(WriteNode node)
        {
            PreVisit(node);
            node.ExprList.Visit(this);
            PostVisit(node);
        }
        public override void Visit(ReadNode node)
        {
            PreVisit(node);
            node.Ident.Visit(this);
            PostVisit(node);
        }
        public override void Visit(BlockNode node)
        {
            PreVisit(node);
            for (int i = 0; i < node.StList.Count; ++i)
            {
                node.StList[i].Visit(this);
            }
            PostVisit(node);
        }
        public override void Visit(DeclarationNode node)
        {
            PreVisit(node);
            foreach (var i in node.IdentList)
            {
                i.Visit(this);
            }
            PostVisit(node);
        }
        public override void Visit(ListExprNode node)
        {
            PreVisit(node);
            foreach (var e in node.ExprList)
            {
                e.Visit(this);
            }
            PostVisit(node);
        }
    }
}
