using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang.Visitors.ChangeVisitors
{
    public class ChangeVisitor : AutoVisitorInversedOrder
    {
        public bool IsChanged = false;
        public void ReplaceExpr(ExprNode from, ExprNode to)
        {
            IsChanged = true;
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
                            break;
                        }
                    }
                    break;
                case ListExprNode listExprNode:
                    for (int i = 0; i < listExprNode.ExprList.Count; ++i)
                    {
                        if (listExprNode.ExprList[i] == from)
                        {
                            listExprNode.ExprList[i] = to;
                            break;
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("ReplaceExpr: wrong parent type");
            }
        }

        public void ReplaceStatement(StatementNode from, StatementNode to)
        {
            IsChanged = true;
            var p = from.Parent;
            to.Parent = p;
            switch (p)
            {
                case ForNode forNode:
                    forNode.Stat = to as BlockNode;
                    break;
                case WhileNode whileNode:
                    whileNode.Stat = to as BlockNode;
                    break;
                case LabeledStatementNode labeledStatement:
                    labeledStatement.Stat = to;
                    break;
                case IfNode ifNode:
                    if (ifNode.Stat == from)
                    {
                        ifNode.Stat = to as BlockNode;
                    } 
                    else if (ifNode.ElseStat == from)
                    {
                        ifNode.ElseStat = to as BlockNode;
                    }
                    break;
                case BlockNode blockNode:
                    for (int i = 0; i < blockNode.StList.Count; ++i)
                    {
                        if (blockNode.StList[i] == from)
                        {
                            blockNode.StList[i] = to;
                            break;
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("ReplaceStatement: wrong parent type");
            }
        }
    }
}
