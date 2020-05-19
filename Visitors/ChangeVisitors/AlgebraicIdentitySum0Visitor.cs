using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class AlgebraicIdentitySum0Visitor : ChangeVisitor
    {
        public override void Visit(BinExprNode BinOp)
        {
            if (BinOp.Left is IntNumNode && (BinOp.Left as IntNumNode).Num == 0)
            {
                BinOp.Right.Visit(this);
                ReplaceExpr(BinOp, BinOp.Right);
            }
            else if (BinOp.Right is IntNumNode && (BinOp.Right as IntNumNode).Num == 0)
            {
                BinOp.Left.Visit(this);
                ReplaceExpr(BinOp, BinOp.Left);
            }
            else base.Visit(BinOp);
        }
    }
}
