using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class WhileFalseVisitor : ChangeVisitor
    {
        public override void Visit(WhileNode node)
        {
            if (node.Condition is BoolValNode bv && bv.Val == false)
            {
                ReplaceStatement(node, new EmptyStatement());
            }
            else
            {
                base.Visit(node);
            }
        }

        public override void PostVisit(Node n)
        {
            if (n is BlockNode bl)
            {
                var prevLength = bl.StList.Count;
                bl.StList = bl.StList.Where(x => !(x is EmptyStatement)).ToList();
                IsChanged = prevLength != bl.StList.Count;
            }
        }
    }
}
