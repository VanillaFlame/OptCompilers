using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class RemoveEmptyStatementVisitor : ChangeVisitor
    {
        public override void Visit(BlockNode node)
        {
            var prevLength = node.StList.Count;
            node.StList = node.StList.Where(x => !(x is EmptyStatement)).ToList();
            IsChanged = prevLength != node.StList.Count;
            base.Visit(node);
        }
    }
}
