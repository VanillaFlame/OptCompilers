using ProgramTree;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class NumberEqualityVisitor : ChangeVisitor
    {
        public override void Visit(BinExprNode binExpr)
        {
            double? number1 = null, number2 = null;
            bool isChanged = false;
            if (binExpr.Left is IntNumNode || binExpr.Left is FloatNumNode)
            {
                if (binExpr.Left is IntNumNode i)
                    number1 = i.Num;
                if (binExpr.Left is FloatNumNode f)
                    number1 = f.Num;
            }
            if (binExpr.Right is IntNumNode || binExpr.Right is FloatNumNode)
            {
                if (binExpr.Right is IntNumNode i)
                    number2 = i.Num;
                if (binExpr.Right is FloatNumNode f)
                    number2 = f.Num;
            }
            if (number1 != null && number2 != null)
            {
                BoolValNode newExpr = null;
                if (binExpr.OpType == BinOpType.Equal)
                {
                    newExpr = new BoolValNode(number1 == number2);
                    ReplaceExpr(binExpr, newExpr);
                }
                    
                if (binExpr.OpType == BinOpType.NotEqual)
                {
                    newExpr = new BoolValNode(number1 != number2);
                    ReplaceExpr(binExpr, newExpr);
                }

                if (binExpr.OpType == BinOpType.Less)
                {
                    newExpr = new BoolValNode(number1 < number2);
                    ReplaceExpr(binExpr, newExpr);
                }

                if (binExpr.OpType == BinOpType.Greater)
                {
                    newExpr = new BoolValNode(number1 > number2);
                    ReplaceExpr(binExpr, newExpr);
                }

                if (binExpr.OpType == BinOpType.LessOrEqual)
                {
                    newExpr = new BoolValNode(number1 <= number2);
                    ReplaceExpr(binExpr, newExpr);
                }

                if (binExpr.OpType == BinOpType.GreaterOrEqual)
                {
                    newExpr = new BoolValNode(number1 >= number2);
                    ReplaceExpr(binExpr, newExpr);
                }

                if (newExpr != null)
                {
                    isChanged = true;
                    base.Visit(newExpr);
                }
            }
            if (!isChanged)
            {
                base.Visit(binExpr);
            }
        }
    }
}
