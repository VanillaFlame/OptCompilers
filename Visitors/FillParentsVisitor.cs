using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors
{
    public class FillParentsVisitor : EnterExitVisitor
    {
        private Stack<Node> stack = new Stack<Node>();

        public FillParentsVisitor()
        {
            stack.Push(null);
        }

        public override void OnEnter(Node node)
        {
            node.Parent = stack.Peek();
            stack.Push(node);
        }

        public override void OnExit(Node node)
        {
            stack.Pop();
        }
    }
}
