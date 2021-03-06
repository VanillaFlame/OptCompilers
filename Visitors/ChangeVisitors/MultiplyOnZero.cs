﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;
using SimpleLang.Visitors.ChangeVisitors;

namespace SimpleLang.Visitors
{
    public class MultiplyOnZero : ChangeVisitor
    {
        public override void PostVisit(Node binop)
        {
            if (binop is BinExprNode bi)
            {
                if (bi.Left is ExprNode && bi.Right is IntNumNode && ((bi.Right as IntNumNode).Num == 0)
                   && bi.OpType == BinOpType.Prod)
                {
                    bi.Right.Visit(this);
                    ReplaceExpr(bi, new IntNumNode(0));
                    IsChanged = true;
                }
                else { 
                    IsChanged = false;
                }
            }
        }
    }
}
