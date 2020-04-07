using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class EnterExitVisitor : AutoVisitor
    {
        public virtual void OnEnter(Node node)
        {

        }

        public virtual void OnExit(Node node)
        {

        }

        public override void Visit(BinExprNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(UnoExprNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(AssignNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(ForNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(WhileNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(LabeledStatementNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(IfNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(WriteNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(ReadNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(BlockNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(DeclarationNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(ListExprNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(BoolValNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(FloatNumNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(GotoNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(IdNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
        public override void Visit(IntNumNode node)
        {
            OnEnter(node);
            base.Visit(node);
            OnExit(node);
        }
    }
}
