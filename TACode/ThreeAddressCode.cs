using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SimpleLang.TACode.TacNodes;
using ProgramTree;
using System;

namespace SimpleLang.TACode
{

   public class ThreeAddressCode: IEnumerable<TacNode>
    {

       public LinkedList<TacNode> TACodeLines { get; }

        public TacNode this[string label]
        {
            get => GetNodeByLabel(label);
            set => SetNodeByLabel(label, value);
        }
        
        public LinkedListNode<TacNode> First => TACodeLines.First;
        public LinkedListNode<TacNode> Last => TACodeLines.Last;

        public ThreeAddressCode()
        {
            TACodeLines = new LinkedList<TacNode>();
        }

       public void PushNode(TacNode node)
        {
            TACodeLines.AddLast(node);
        }

       public void PushNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.AddLast(tacNode);
            }
        }

       public void RemoveNode(TacNode node)
        {
            TACodeLines.Remove(node);
        }

       public void RemoveNodeByLabel(string label)
        {
            var labeledNode = TACodeLines.FirstOrDefault(node => string.Equals(node.Label, label));
            if (labeledNode != null)
            {
                TACodeLines.Remove(labeledNode);
            }
        }

       public void RemoveNodes(IEnumerable<TacNode> nodes)
        {
            foreach (var tacNode in nodes)
            {
                TACodeLines.Remove(tacNode);
            }
        }

       public TacNode GetNodeByLabel(string label)
        {
            return TACodeLines.FirstOrDefault(node => string.Equals(node.Label, label));
        }

       public void SetNodeByLabel(string label, TacNode value)
        {
            var current = TACodeLines.First;
            while (current != null) //currnet.next != null
            {
                if (!string.Equals(current.Value.Label, label))
                {
                    current = current.Next;
                    continue;
                }
                current.Value = value;
                return;
            }
            PushNode(value);
        }

        #region Convenience methods

       public string CreateAndPushBoolNode(BoolValNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Val.ToString()
            });
            return tmpName;
        }

       public string CreateAndPushIdNode(IdNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Name.ToString()
            });
            return tmpName;
        }

       public string CreateAndPushIntNumNode(IntNumNode node, string label=null)
        {
            var tmpName = TmpNameManager.Instance.GenerateTmpVariableName();
            PushNode(new TacAssignmentNode()
            {
                Label = label,
                LeftPartIdentifier = tmpName,
                FirstOperand = node.Num.ToString()
            });
            return tmpName;
        }

       public void CreateAndPushEmptyNode(EmptyNode node, string label=null)
        {
            PushNode(new TacEmptyNode()
            {
                Label = label
            });
        }

        #endregion

        #region utility methods

        public static bool IsFunction(string operand)
        {
            return operand.StartsWith("func");
        }

        #endregion
        public IEnumerator<TacNode> GetEnumerator()
        {
            return TACodeLines.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return TACodeLines.GetEnumerator();
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var tacNode in TACodeLines)
            {
                builder.Append(tacNode?.ToString() + "\n");
            }

            return builder.ToString();
        }
        
        public string ToString1()
        {
            var builder = new StringBuilder();
            foreach (var tacNode in TACodeLines)
            {
                builder.Append(tacNode?.ToString() + "\\n");
            }

            return builder.ToString();
        }
    }
}
