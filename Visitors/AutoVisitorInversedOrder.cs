using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class AutoVisitorInversedOrder : AutoVisitor
    {
        public override void Visit(BlockNode node)
        { 
            for (int i = node.StList.Count - 1; i >= 0; --i)
            {
                node.StList[i].Visit(this);
            }
        }
    }
}
