using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class TrueConditionOptVisitor : ChangeVisitor
    {
        public override void Visit(BinExprNode node)
        {
            if (node.Left is IdNode leftIdNode && node.Right is IdNode rightIdNode
                && leftIdNode.Name.Equals(rightIdNode.Name)
                && (node.OpType == BinOpType.Equal
                    || node.OpType == BinOpType.LessOrEqual
                    || node.OpType == BinOpType.GreaterOrEqual))
            {
                ReplaceExpr(node, new BoolValNode(true));
            }
        }
    }
}
