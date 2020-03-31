using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    class AssignCountVisitor : AutoVisitor
    {
        public int AssignCount;
        public override void Visit(AssignNode node)
        {
            ++AssignCount;
        }
    }
}
