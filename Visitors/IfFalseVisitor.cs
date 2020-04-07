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

/*namespace SimpleLang.Visitors
{
    class IfFalseVisitor : AutoVisitor
    {
        /*public void ReplaceStat(StatNode from, StatNode to)
        {            
            var p = from.Parent;
            if (p is AssignNode || p is ExprNode)
            {
                throw new Exception("Родительский узел не содержит операторов");
            }
            to.Parent = p;
            if (p is BlockNode bln) // Можно переложить этот код на узлы!
            {
                for (var i = 0; i < bln.lst.Count; i++)
                    if (bln.lst[i] == from)
                        bln.lst[i] = to;
            }
            else if (p is IfNode ifn)
            {
                if (ifn.ThenStat == from) // Поиск подузла в Parent
                    ifn.ThenStat = to;
                else if (ifn.ElseStat == from)
                    ifn.ElseStat = to;
            }
        }

        public override void Visit(BinExprNode node)
        {
            node.Left.Visit(this);
            node.Right.Visit(this);
        }
        public override void Visit(BinExprNode ass)
        {
            if (ass.Expr is IdNode idn && ass.Id.Name == idn.Name)
            {
                ReplaceStat(ass, null); // Заменить на null.

                // Потом этот null надо специально проверять!!!

            }
            // Не обходить потомков
        }
        public override void VisitIfNode(IfNode ifn)
        {
            if (ifn.Expr is BoolNumNode bnn && bnn.Value == "false")
            {
                if (ifn.ElseStat != null)
                    ifn.ElseStat.Visit(this); // Вначале обойти потомка
                ReplaceStat(ifn, ifn.ElseStat);
            }
            else if (ifn.Expr is BoolNumNode bnn && bnn.Value == "true")
            {
                if (ifn.ThenStat != null)
                    ifn.ThenStat.Visit(this); // Вначале обойти потомка
                ReplaceStat(ifn, ifn.ThenStat);
            }
            else
            {
                base.VisitIfNode(ifn);
            }
        }
    }
}*/
