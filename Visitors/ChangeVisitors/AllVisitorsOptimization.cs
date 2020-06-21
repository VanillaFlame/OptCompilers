using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;
using SimpleLang.Visitors.ChangeVisitors;
using SimpleParser;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public static class AllVisitorsOptimization
    {
        public static List<ChangeVisitor> ChangeVisitorsOptimization = new List<ChangeVisitor>
        {
            new SameAssignmentOptVisitor(),
            new NumberEqualityVisitor(),
            new MinusSelf(),
            new FindFalseVisitor(),
            new WhileFalseVisitor(),
            new MultiplyOnZero(),
            new MultiplyOnZero2(),
            new TrueConditionOptVisitor(),
            new TrueIfOptVisitor(),
            new NullIfElseOptVisitor(),
            new RemoveEmptyStatementVisitor(),
            new ConstantFoldingVisitor(),
            new IfFalseVisitor(),
            new AlgebraicIdentityProdDiv1Visitor(),
            new AlgebraicIdentitySum0Visitor()
        };

        public static void Optimization(Parser parser)
        {
            int countOptimization = 0;
            while (countOptimization < ChangeVisitorsOptimization.Count)
            {
                parser.root.Visit(ChangeVisitorsOptimization[countOptimization]);
                if (ChangeVisitorsOptimization[countOptimization].IsChanged)
                {
                    ChangeVisitorsOptimization[countOptimization].IsChanged = false;
                    countOptimization = 0;
                }
                else countOptimization++;
            }
        }
    }
}
