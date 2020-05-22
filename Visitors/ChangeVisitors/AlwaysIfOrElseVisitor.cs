using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class AlwaysIfOrElseVisitor : ChangeVisitor
    {
        public override void Visit(IfNode ifnode)
        {
            if (ifnode.Condition is BoolValNode expr)
            {
                if (expr.Val == true)
                {
                    ReplaceStatement(ifnode, ifnode.Stat);
                    base.Visit(ifnode.Stat);
                }
                else
                {
                    ReplaceStatement(ifnode, ifnode.ElseStat);
                    base.Visit(ifnode.ElseStat);
                }
            }     
            else
            {
                base.Visit(ifnode);
            }
        }

    }
}
