using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class ChangeVisitor : AutoVisitor
    {
        public void ReplaceExpr(ExprNode from, ExprNode to)
        {
            var p = from.Parent;
            to.Parent = p;
            switch (p) {
                case AssignNode ass:
                    ass.Expr = to;
                    break;
                case BinExprNode binExp:
                    if (binExp.Left == from)
                    {
                        binExp.Left = to;
                    }
                    else if (binExp.Right == from)
                    {
                        binExp.Right = to;
                    }
                    break;
                case UnoExprNode unoExpr:
                    unoExpr.Expr = to;
                    break;
                case ForNode forNode:
                    if (forNode.Begin == from)
                    {
                        forNode.Begin = to;
                    }
                    if (forNode.End == from)
                    {
                        forNode.End = to;
                    }
                    if (forNode.Counter == from)
                    {
                        forNode.Counter = to as IdNode;
                    }
                    break;
                case WhileNode whileNode:
                    whileNode.Condition = to;
                    break;
                case IfNode ifNode:
                    ifNode.Condition = to;
                    break;
                case ReadNode readNode:
                    readNode.Ident = to as IdNode;
                    break;
                case DeclarationNode declarationNode:
                    for (int i = 0; i < declarationNode.IdentList.Count; ++i)
                    {
                        if (declarationNode.IdentList[i] == to)
                        {
                            declarationNode.IdentList[i] = to as IdNode;
                        }
                    }
                    break;
                case ListExprNode listExprNode:
                    for (int i = 0; i < listExprNode.ExprList.Count; ++i)
                    {
                        listExprNode.ExprList[i] = to;
                    }
                    break;
                default:
                    throw new ArgumentException("ReplaceExpr: wrong parent type");
            }
        }
    }
}
