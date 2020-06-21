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
                    base.Visit(ifnode.Stat);
                    ReplaceStatement(ifnode, ifnode.Stat);
                }
                else
                {
                    base.Visit(ifnode.ElseStat);
                    ReplaceStatement(ifnode, ifnode.ElseStat);
                }
            }     
            else
            {
                base.Visit(ifnode);
            }
        }

    }
}
