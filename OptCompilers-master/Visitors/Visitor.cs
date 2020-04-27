using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public abstract class Visitor
    {
        public virtual void Visit(IdNode node)
        { }
        public virtual void Visit(IntNumNode node)
        { }
        public virtual void Visit(FloatNumNode node)
        { }
        public virtual void Visit(BoolValNode node)
        { }
        public virtual void Visit(BinExprNode node)
        { }
        public virtual void Visit(UnoExprNode node)
        { }
        public virtual void Visit(AssignNode node)
        { }
        public virtual void Visit(ForNode node)
        { }
        public virtual void Visit(WhileNode node)
        { }
        public virtual void Visit(GotoNode node)
        { }
        public virtual void Visit(LabeledStatementNode node)
        { }
        public virtual void Visit(IfNode node)
        { }
        public virtual void Visit(WriteNode node)
        { }
        public virtual void Visit(ReadNode node)
        { }
        public virtual void Visit(BlockNode node)
        { }
        public virtual void Visit(DeclarationNode node)
        { }
        public virtual void Visit(ListExprNode node)
        { }
    }
}
