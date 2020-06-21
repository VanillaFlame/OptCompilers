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
                if (a.Expr is IdNode && (a.Expr as IdNode).Name == a.Id.Name)
                {
                    ReplaceStatement(a, null);
                }
            }
            else if (n is BlockNode bl)
            {
                bl.StList = bl.StList.Where(x => x != null).ToList();
            }

        }
    }
}
