using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class TrueIfOptVisitor : ChangeVisitor
    {
        public override void Visit(IfNode node)
        {
            if (node.Condition is BoolValNode bv && bv.Val)
            {
                Visit(node.Stat);
                ReplaceStatement(node, node.Stat);
                IsChanged = true;
            }
            else
            {
                IsChanged = false;
                base.Visit(node);
            }
        }
    }
}
