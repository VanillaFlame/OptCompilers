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
    }
}
