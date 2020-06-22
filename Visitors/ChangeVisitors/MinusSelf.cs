using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class MinusSelf : ChangeVisitor
    {
        public override void PostVisit(Node binop)
        {
            if (binop is BinExprNode bi)
            {
                if (bi.Left is IdNode && bi.Right is IdNode && ((bi.Left as IdNode).Name == (bi.Right as IdNode).Name)
                && bi.OpType == BinOpType.Minus)
                {
                    ReplaceExpr(bi, new IntNumNode(0));
                    IsChanged = true;
                }
                else
                {
                    IsChanged = false;
                }
            }
        }
    }
}
