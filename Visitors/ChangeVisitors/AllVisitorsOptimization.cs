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
            new MinisSelf(),
            new FindFalseVisitor(),
            new MultiplayOnZero(),
            new MultiplayOnZero2(),
            new TrueConditionOptVisitor(),
            new TrueIfOptVisitor(),
            new IfFalseVisitor()
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
