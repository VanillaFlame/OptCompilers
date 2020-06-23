using System;
using System.Collections.Generic;
using System.Linq;
using SimpleLang.TAC;

namespace SimpleLang.CFG
{
    public enum EdgeType
    {
        None = 0,
        Advancing = 1,
        Retreating = 2,
        Cross = 3
    }

    /// <summary>
    /// Заготовка
    /// Вписать типы всех граней в глубинном остовном дереве
    /// </summary>
    public static class CFGExtension
    {
        static bool isPathExists(int start, int end, IEnumerable<IndexEdge> ostovTreeEdges)
        {
            var visitedChildren = new HashSet<int>();
            var children = new HashSet<int>();
            children.Add(start);
            while (children.Count != 0)
            {
                visitedChildren.UnionWith(children);

                var oldChildren = new HashSet<int>(children);
                foreach (var child in oldChildren)
                    //children.UnionWith(ostovTreeEdges.Where(e => e.start == child).Select(e => e.end));
                    children.UnionWith(ostovTreeEdges.Where(e => e.start == child || e.end == child)
                        .Select(e => e.start == child? e.end : e.start));

                if (children.Contains(end))
                    return true;

                children.ExceptWith(visitedChildren);
            }
            
            return false;
        }

        public static Dictionary<IndexEdge, EdgeType> classifyEdges(this ControlFlowGraph c)
        {
            var types = new Dictionary<IndexEdge, EdgeType>();
            var ostovTree = new SpanningTree(c);

            var edges = new HashSet<IndexEdge>();
            edges.Add(new IndexEdge(c.start.Index, c.blocks[0].Index));
            for (int i = 0; i < c.blocks.Count; i++)
            {
                edges.UnionWith(c.blocks[i].Out.Select(b =>
                    new IndexEdge(c.blocks[i].Index, b.Index)));
            }

            foreach (var edge in edges)
            {
                // если одного из узлов вообще нет в остовном
                if (!ostovTree.dfn.ContainsKey(edge.start) || 
                    !ostovTree.dfn.ContainsKey(edge.end))
                    types[edge] = EdgeType.Cross;
                else if (ostovTree.dfst.Any(e => e.start == edge.start && e.end == edge.end) 
                    || (ostovTree.dfn[edge.start] < ostovTree.dfn[edge.end] && isPathExists(edge.start, edge.end, ostovTree.dfst)))
                {
                    types[edge] = EdgeType.Advancing;
                }
                else if (ostovTree.dfn[edge.start] >= ostovTree.dfn[edge.end]
                    && isPathExists(edge.start, edge.end, ostovTree.dfst))
                {
                    types[edge] = EdgeType.Retreating;
                }
                else
                {
                    types[edge] = EdgeType.Cross;
                }
            }            return types;
        }
    }
}
