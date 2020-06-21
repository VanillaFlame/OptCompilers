using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class NullIfElseOptVisitor: ChangeVisitor
    {
        public override void PostVisit(Node n)
        {
            if (n is IfNode ifN)
            {
                if ((ifN.Stat.StList.Count == 0 && ifN.ElseStat == null) ||
                    (ifN.Stat.StList.Count == 0 && ifN.ElseStat.StList.Count == 0))
                {
                    ReplaceStatement(ifN, null);
                }
                if (ifN.ElseStat != null && ifN.ElseStat.StList.Count == 0)
                {
                    ReplaceStatement(ifN, new IfNode(ifN.Condition, ifN.Stat));
                }
            }
            else if (n is BlockNode bl)
            {
                bl.StList = bl.StList.Where(x => x != null).ToList();
            }
        }
    }
}
