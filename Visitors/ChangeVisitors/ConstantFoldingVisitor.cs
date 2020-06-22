using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class ConstantFoldingVisitor : ChangeVisitor
    {
        public override void PostVisit(Node n)
        {
            if (n is BinExprNode node)
            {
                if (node.Left is IntNumNode leftInt && node.Right is IntNumNode rightInt)
                {
                    if (node.OpType == BinOpType.Div)
                    {
                        ReplaceExpr(node, new IntNumNode(leftInt.Num / rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Prod)
                    {
                        ReplaceExpr(node, new IntNumNode(leftInt.Num * rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Minus)
                    {
                        ReplaceExpr(node, new IntNumNode(leftInt.Num - rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Plus)
                    {
                        ReplaceExpr(node, new IntNumNode(leftInt.Num + rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Less)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num < rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Greater)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num > rightInt.Num));
                    }
                    if (node.OpType == BinOpType.LessOrEqual)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num <= rightInt.Num));
                    }
                    if (node.OpType == BinOpType.GreaterOrEqual)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num >= rightInt.Num));
                    }
                    if (node.OpType == BinOpType.Equal)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num == rightInt.Num));
                    }
                    if (node.OpType == BinOpType.NotEqual)
                    {
                        ReplaceExpr(node, new BoolValNode(leftInt.Num != rightInt.Num));
                    }
                }
            }
        }
    }
}
