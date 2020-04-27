using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SimpleLang.Visitors.ChangeVisitors
{
        public class IfFalseVisitor : ChangeVisitor
        {
            public override void Visit(IfNode node)
            {
                if (node.Condition is BoolValNode bv && !bv.Val)
                {
                    Visit(node.ElseStat);
                    ReplaceStatement(node, node.ElseStat);
                }
                else
                {
                    base.Visit(node);
                }
            }
        }       
}
