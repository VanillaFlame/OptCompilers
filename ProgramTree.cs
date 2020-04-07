using SimpleLang;
using System.Collections.Generic;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public enum BinOpType { Or, And, Plus, Minus, Prod, Div, Less, LessOrEqual, Greater, GreaterOrEqual, Equal, NotEqual };

    public enum UnoOpType { Minus, Not };

    public enum VariableType { Int, Float, Bool }

    public static class AssignTypeExtensions
    {
        public static string ToFriendlyString(this AssignType me)
        {
            switch (me)
            {
                case AssignType.Assign:
                    return "=";
                case AssignType.AssignPlus:
                    return "+=";
                case AssignType.AssignMult:
                    return "*=";
                case AssignType.AssignMinus:
                    return "-=";
                case AssignType.AssignDivide:
                    return "/=";
                default:
                    return "ErrorAssignType";
            }
        }
    }

    public static class BinOpTypeExtensions
    {
        public static string ToFriendlyString(this BinOpType me)
        {
            switch (me)
            {
                case BinOpType.Or:
                    return "|";
                case BinOpType.And:
                    return "&";
                case BinOpType.Plus:
                    return "+";
                case BinOpType.Minus:
                    return "-";
                case BinOpType.Prod:
                    return "*";
                case BinOpType.Div:
                    return "/";
                case BinOpType.Less:
                    return "<";
                case BinOpType.LessOrEqual:
                    return "<=";
                case BinOpType.Greater:
                    return ">";
                case BinOpType.GreaterOrEqual:
                    return ">=";
                case BinOpType.Equal:
                    return "==";
                case BinOpType.NotEqual:
                    return "!=";
                default:
                    return "ErrorBinOpType";
            }
        }
    }

    public static class UnoOpTypeExtensions
    {
        public static string ToFriendlyString(this UnoOpType me)
        {
            switch (me)
            {
                case UnoOpType.Minus:
                    return "-";
                case UnoOpType.Not:
                    return "!";
                default:
                    return "ErrorUnoOpType";
            }
        }
    }

    public abstract class Node // базовый класс для всех узлов    
    {
        public abstract void Visit(Visitor v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class FloatNumNode : ExprNode
    {
        public double Num { get; set; }
        public FloatNumNode(double num) { Num = num; }
        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class BoolValNode : ExprNode
    {
        public bool Val { get; set; }
        public BoolValNode(bool val) { Val = val; }
        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class ListExprNode : Node
    {
        public List<ExprNode> ExprList { get; set; } = new List<ExprNode>();
        public ListExprNode(ExprNode expr)
        {
            ExprList.Add(expr);
        }

        public void Add(ExprNode expr)
        {
            ExprList.Add(expr);
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class BinExprNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public BinOpType OpType { get; set; }
        public BinExprNode(ExprNode left, ExprNode right, BinOpType type)
        {
            Left = left;
            Right = right;
            OpType = type;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class UnoExprNode : ExprNode
    {
        public ExprNode Expr { get; set; }
        public UnoOpType OpType { get; set; }
        public UnoExprNode(ExprNode expr, UnoOpType type)
        {
            Expr = expr;
            OpType = type;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class ForNode : StatementNode
    {
        public IdNode Counter { get; set; }
        public ExprNode Begin { get; set; }
        public ExprNode End { get; set; }
        public BlockNode Stat { get; set; }
        public ForNode(IdNode counter, ExprNode begin, ExprNode end, BlockNode stat)
        {
            Counter = counter;
            Begin = begin;
            End = end;
            Stat = stat;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class WhileNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public BlockNode Stat { get; set; }
        public WhileNode(ExprNode condition, BlockNode stat)
        {
            Condition = condition;
            Stat = stat;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class GotoNode : StatementNode
    {
        public int Label { get; set; }
        public GotoNode(int label)
        {
            Label = label;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class LabeledStatementNode : StatementNode
    {
        public int Label { get; set; }
        public StatementNode Stat { get; set; }
        public LabeledStatementNode(int label, StatementNode stat)
        {
            Label = label;
            Stat = stat;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public BlockNode Stat { get; set; }
        public BlockNode ElseStat { get; set; }
        public IfNode(ExprNode condition, BlockNode stat)
        {
            Condition = condition;
            Stat = stat;
        }

        public IfNode(ExprNode condition, BlockNode stat, BlockNode elseStat)
        {
            Condition = condition;
            Stat = stat;
            ElseStat = elseStat;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class WriteNode : StatementNode
    {
        public ListExprNode ExprList { get; set; }
        public bool WriteLine { get; set; }

        public WriteNode(ListExprNode exprList, bool writeLine)
        {
            ExprList = exprList;
            WriteLine = writeLine;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class ReadNode : StatementNode
    {
        public IdNode Ident { get; set; }
        public ReadNode(IdNode ident)
        {
            Ident = ident;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class DeclarationNode : StatementNode
    {
        public List<IdNode> IdentList { get; set; }

        public VariableType Type { get; set; }

        public DeclarationNode(List<IdNode> identList, VariableType type)
        {
            IdentList = identList;
            Type = type;
        }

        public override void Visit(Visitor v)
        {
            v.Visit(this);
        }
    }
}