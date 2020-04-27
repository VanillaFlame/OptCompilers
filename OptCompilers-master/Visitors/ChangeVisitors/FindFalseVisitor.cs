using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class FindFalseVisitor : ChangeVisitor
    {
        public override void Visit(BinExprNode node)
        {
            if (node.Left is IdNode leftIdNode && node.Right is IdNode rightIdNode
                && leftIdNode.Name.Equals(rightIdNode.Name)
                && (node.OpType == BinOpType.Greater
                    || node.OpType == BinOpType.Less
                    || node.OpType == BinOpType.NotEqual))
            {
                ReplaceExpr(node, new BoolValNode(false));
            } 
            else
            {
                base.Visit(node);
            }
        }
    }
}
