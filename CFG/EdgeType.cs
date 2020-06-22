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
        static bool isPathExists(BasicBlock start, BasicBlock end)
        {
            // писалось как от предка к ребенку, но и наоборот работает
            var visitedChildren = new HashSet<BasicBlock>();
            var children = new HashSet<BasicBlock>();
            children.Add(start);
            while (children.Count != 0)
            {
                visitedChildren.UnionWith(children);

                foreach (var child in children)
                {
                    if (start.Index < end.Index)
                        children.UnionWith(child.Out);
                    else
                        children.UnionWith(child.In);
                }
             
                if (children.Contains(end))
                    return true;

                children.ExceptWith(visitedChildren);
            }
            
            return false;
        }

        public static Dictionary<BlockEdge, EdgeType> classifyEdges(this ControlFlowGraph c)
        {
            var types = new Dictionary<BlockEdge, EdgeType>();
            var ostovTree = new Ostov_Tree(c);
            var edges = new HashSet<BlockEdge>();
            edges.Add(new BlockEdge(c.start, c.blocks[0]));
            for (int i = 0; i < c.blocks.Count; i++)
            {
                edges.UnionWith(c.blocks[i].Out.Select(b =>
                    new BlockEdge(c.blocks[i], b)));
            }

            foreach (var edge in edges)
            {
                if (ostovTree.dfst.Any(e => e.start == edge.start.Index
                    && e.end == edge.end.Index) 
                    || (edge.start.Index < edge.end.Index && isPathExists(edge.start, edge.end)))
                {
                    types[edge] = EdgeType.Advancing;
                }
                else if (isPathExists(edge.start, edge.end))
                {
                    types[edge] = EdgeType.Retreating;
                }
                else
                {
                    throw new Exception();
                    // если Володя не обрабатывает грани от старта и к енду, то тут надо их вручную обработать
                    types[edge] = EdgeType.Cross;
                }
            }            return types;
        }
    }
}
