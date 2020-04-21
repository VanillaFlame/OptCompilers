using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;
namespace SimpleLang.Visitors.ChangeVisitors
{
    class TreeLeverCodeVisitor:AutoVisitor
    {
        List<string> progr =new List<string>();
        public override void Visit(BlockNode node)
        {
            for (int i = 0; i < node.StList.Count; ++i)
            {
                node.StList[i].Visit(this);
            }
        }
        public void PrintProgramm()
        {
            for (var i= 0;i < progr.Count; ++i)
            {
                Console.WriteLine(i.ToString()+": "+progr[i]);
            }
        }
        int tmpnum = 0;
        private string genTmpName()
        {
            return "t" + (tmpnum++).ToString();
        }
        int labelnum = 0;
        private string genlabelName()
        {
            return "label" + (labelnum++).ToString();
        }
        public override void Visit(AssignNode node)
        {
            string tmp = gen(node.Expr);
            genCommand(node.Id.Name + " = " + tmp);
        }
        private void genCommand(string s)
        {
            progr.Add(s);
        }


        string gen(ExprNode ex)
        {
            if (ex.GetType() == typeof(BinExprNode))
            {
                var bin = (BinExprNode)ex;
                string tmp1 = gen(bin.Left);
                string tmp2 = gen(bin.Right);
                string tmp = genTmpName();
                genCommand(tmp + " = " + tmp1 +" " + bin.OpType.ToString() + " " + tmp2);
                return tmp;
            }
            if (ex.GetType() == typeof(IdNode))
            {
                var bin = (IdNode)ex;
                return bin.Name;
            }
            if (ex.GetType() == typeof(IntNumNode))
            {
                var bin = (IntNumNode)ex;
                return bin.Num.ToString();
            }
            return "";
        }

        public override void Visit(IfNode node)
        {
            string tmp = gen(node.Condition);
            string L1 = genlabelName();
            string L2 = genlabelName();
            genCommand("if " + tmp + " goto " + L1);
            if (node.ElseStat!=null)
            { node.ElseStat.Visit(this);
                genCommand("goto " + L2); }
            genCommand(L1 + ": nop");
            node.Stat.Visit(this);
            genCommand(L2 + ": nop");
        }

        public override void Visit(ForNode node)
        {
            string tmp = genTmpName();
            string tmp1 = gen(node.Begin);
            genCommand( tmp + " = " + tmp1);
            string forStartLever = genlabelName();
            genCommand(forStartLever + ": nop");
            node.Stat.Visit(this);
            string tmp2 = gen(node.End);
            genCommand( tmp+ " = " + tmp + " + 1");
            genCommand("if " + tmp+" < "+ tmp2+ " goto " + forStartLever);
        }

        public override void Visit(UnoExprNode node)
        {
            string tmp = gen(node.Expr);
            genCommand(node.OpType.ToString() + tmp);
        }

        public override void Visit(WriteNode node)
        {
            StringBuilder sb = new StringBuilder( "Write(");
            for (int i = 0; i < node.ExprList.ExprList.Count; ++i)
            {
                string tmp = gen(node.ExprList.ExprList[i]);
                if(i==0)
                sb.Append(tmp) ;
                else
                    sb.Append(", "+tmp);
            }
            sb.Append(")");
            genCommand(sb.ToString());
        }
        public override void Visit(ReadNode node)
        {
            genCommand("Read("+node.Ident.Name+")");
        }
    }
}
