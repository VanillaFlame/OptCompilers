using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class MultiplyOnZero2 : ChangeVisitor
    {
        public override void PostVisit(Node binop)
        {
            if (binop is BinExprNode bi)
            {
                if (bi.Right is ExprNode && bi.Left is IntNumNode && ((bi.Left as IntNumNode).Num == 0)
                            && bi.OpType == BinOpType.Prod)
                {
                    bi.Left.Visit(this);
                    ReplaceExpr(bi, new IntNumNode(0));
                    IsChanged = true;
                }
                else IsChanged = false;
            }
        }
    }
}
