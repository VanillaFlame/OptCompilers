using ProgramTree;
using SimpleLang.Visitors.ChangeVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class AlgebraicIdentityProdDiv1Visitor : ChangeVisitor
    {
        public override void Visit(BinExprNode BinOp)
        {
            switch (BinOp.OpType)
            {
                case BinOpType.Prod:
                    if (BinOp.Left is IntNumNode && (BinOp.Left as IntNumNode).Num == 1)
                    {
                        BinOp.Right.Visit(this);
                        ReplaceExpr(BinOp, BinOp.Right);
                    }
                    else if (BinOp.Right is IntNumNode && (BinOp.Right as IntNumNode).Num == 1)
                    {
                        BinOp.Left.Visit(this);
                        ReplaceExpr(BinOp, BinOp.Left);
                    }
                    else base.Visit(BinOp);
                    break;
                case BinOpType.Div:
                    if (BinOp.Right is IntNumNode && (BinOp.Right as IntNumNode).Num == 1)
                    {
                        BinOp.Left.Visit(this);
                        ReplaceExpr(BinOp, BinOp.Left);
                    }
                    break;
                default:
                    base.Visit(BinOp);
                    break;
            }
        }
    }
}
