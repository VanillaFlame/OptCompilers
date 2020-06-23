using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class AlwaysIfOrElseVisitor : ChangeVisitor
    {
        public override void Visit(IfNode ifnode)
        {
            if (ifnode.Condition is BoolValNode expr)
            {
                if (expr.Val == true)
                {
                    Visit(ifnode.Stat);
                    ReplaceStatement(ifnode, ifnode.Stat);
                }
                else
                {
                    if (ifnode.ElseStat != null)
                    {
                        Visit(ifnode.ElseStat);
                        ReplaceStatement(ifnode, ifnode.ElseStat);
                    }
                }
            }
            base.Visit(ifnode);
        }
    }
}
