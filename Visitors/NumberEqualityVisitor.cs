using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    class NumberEqualityVisitor : ChangeVisitor
    {
        public override void Visit(BinExprNode binExpr)
        {
            base.Visit(binExpr);
            if (binExpr.Left is IntNumNode left && binExpr.Right is IntNumNode right)
            {
                if (binExpr.OpType == BinOpType.Equal)
                    ReplaceExpr(binExpr, new BoolValNode(left.Num == right.Num));
                if (binExpr.OpType == BinOpType.NotEqual)
                    ReplaceExpr(binExpr, new BoolValNode(left.Num != right.Num));
            } 
        }
    }
}
