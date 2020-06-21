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
                if (node.ElseStat != null)
                {
                    Visit(node.ElseStat);
                    ReplaceStatement(node, node.ElseStat);
                } else
                {
                    ReplaceStatement(node, new EmptyStatement());
                }
                IsChanged = true;
            }
            else
            {
                IsChanged = false;
                base.Visit(node);
            }
        }

        public override void PostVisit(Node n)
        {
            if (n is BlockNode bl)
            {
                bl.StList = bl.StList.Where(x => !(x is EmptyStatement)).ToList();
            }
        }
    }
}
