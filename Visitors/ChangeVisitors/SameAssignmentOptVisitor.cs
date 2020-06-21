using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class SameAssignmentOptVisitor: ChangeVisitor
    {
        public override void PostVisit(Node n)
        {
            if (n is AssignNode a)
            {
                if (a.Expr is IdNode idNode && idNode.Name == a.Id.Name)
                {
                    ReplaceStatement(a, new EmptyStatement());
                }
            }
            else if (n is BlockNode bl)
            {
                bl.StList = bl.StList.Where(x => !(x is EmptyStatement)).ToList();
            } 
        }
    }
}
