using System.Collections.Generic;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public enum BinOpType { Or, And, Plus, Minus, Prod, Div, Less, LessOrEqual, Greater, GreaterOrEqual, Equal, NotEqual };

    public enum UnoOpType { Minus, Not };

    public enum VariableType { Int, Float, Bool }

    public class Node // базовый класс для всех узлов    
    {
    }

    public class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class BinOpNode : ExprNode
    {

    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
    }

    public class FloatNumNode : ExprNode
    {
        public double Num { get; set; }
        public FloatNumNode(double num) { Num = num; }
    }

    public class BoolValNode : ExprNode
    {
        public bool Val { get; set; }
        public BoolValNode(bool val) { Val = val; }
    }

    public class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class ListExprNode : Node
    {
        public List<ExprNode> ExprList = new List<ExprNode>();
        public ListExprNode(ExprNode expr)
        {
            ExprList.Add(expr);
        }

        public void Add(ExprNode expr)
        {
            ExprList.Add(expr);
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
    }

    public class GotoNode : StatementNode
    {
        public int Label { get; set; }
        public GotoNode(int label)
        {
            Label = label;
        }
    }

    public class LabeledStatementNode : StatementNode
    {
        public int Label { get; set; }
        StatementNode Stat { get; set; }
        public LabeledStatementNode(int label, StatementNode stat)
        {
            Label = label;
            Stat = stat;
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
    }

    public class WriteNode : StatementNode
    {
        public ListExprNode ExprList;
        public bool WriteLine { get; set; }

        public WriteNode(ListExprNode exprList, bool writeLine)
        {
            ExprList = exprList;
            WriteLine = writeLine;
        }
    }

    public class ReadNode : StatementNode
    {
        public IdNode Ident { get; set; }
        public ReadNode(IdNode ident)
        {
            Ident = ident;
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
    }
}