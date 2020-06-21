using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLang.CFG
{
    public enum EdgeType
    {
        None = 0, // не должно понадобиться, но кто знает?
        Advancing = 1,
        Retreating = 2,
        Cross = 3
    }

    /// <summary>
    /// Заготовка
    /// Вписать типы всех граней в глубинном остовном дереве
    /// </summary>
    public static class SpanningTreeExtension
    {      
        /*public static void classifyEdges(this SpanningTree t)
        {
            foreach (var edge in t.Edges)
            {
                if (condition)
                    edge.Type = EdgeType.Advancing;
            }
        }*/
    }
}
