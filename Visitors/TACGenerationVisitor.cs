﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;
using SimpleLang.TAC;

namespace SimpleLang.Visitors
{
    public class TACGenerationVisitor : AutoVisitor
    {
        public List<TACInstruction> Instructions { get; private set; }
        public ThreeAddressCode TAC { get; private set; }

        public TACGenerationVisitor()
        {
            tempVariableCounter = 0;
            tempLabelCounter = 0;
            Instructions = new List<TACInstruction>();
            TAC = new ThreeAddressCode(Instructions);
        }

        private static string TEMP_NAME_PREFIX = "#t";
        private static string TEMP_LABEL_PREFIX = "#L";

        public override void Visit(AssignNode node)
        {
            if (node.Expr is BinExprNode bin
                && !(bin.Left is BinExprNode)
                && !(bin.Right is BinExprNode)
                && !(bin.Left is UnoExprNode)
                && !(bin.Right is UnoExprNode))
            {
                string leftArg = "";
                if (bin.Left is IdNode id)
                {
                    leftArg = id.Name;
                } 
                else if (bin.Left is FloatNumNode f)
                {
                    leftArg = f.Num.ToString();
                }
                else if (bin.Left is BoolValNode b)
                {
                    leftArg = b.Val.ToString();
                }
                else if (bin.Left is IntNumNode i)
                {
                    leftArg = i.Num.ToString();
                }

                string rightArg = "";
                if (bin.Right is IdNode id2)
                {
                    rightArg = id2.Name;
                }
                else if (bin.Right is FloatNumNode f2)
                {
                    rightArg = f2.Num.ToString();
                }
                else if (bin.Right is BoolValNode b2)
                {
                    rightArg = b2.Val.ToString();
                }
                else if (bin.Right is IntNumNode i2)
                {
                    rightArg = i2.Num.ToString();
                }
                AddInstruction(bin.OpType.ToFriendlyString(), leftArg, rightArg, node.Id.Name);
            }
            else
            {
                var tmp = GenerateTACExpr(node.Expr);
                AddInstruction(node.AssOp.ToFriendlyString(), tmp, "", node.Id.Name);
            }
        }

        public override void Visit(IfNode node)
        {
            var genCond = GenerateTACExpr(node.Condition);
            var label1 = GenerateTempLabel();
            var label2 = GenerateTempLabel();
            AddInstruction("if goto", genCond, label1, "");
            if (node.ElseStat != null)
                node.ElseStat.Visit(this);
            AddInstruction("goto", label2, "", "");
            AddInstruction("", "", "", "", label1);
            node.Stat.Visit(this);
            AddInstruction("", "", "", "", label2);
        }

        public override void Visit(WhileNode node)
        {
            var loopBeginLabel = GenerateTempLabel();
            var loopBodyLabel = GenerateTempLabel();
            var loopEndLabel = GenerateTempLabel();
            AddInstruction("", "", "", "", loopBeginLabel);
            var genCond = GenerateTACExpr(node.Condition);
            AddInstruction("if goto", genCond, loopBodyLabel, "");
            AddInstruction("goto", loopEndLabel, "", "");
            AddInstruction("", "", "", "", loopBodyLabel);
            node.Stat.Visit(this);
            AddInstruction("goto", loopBeginLabel, "", "");
            AddInstruction("", "", "", "", loopEndLabel);
        }

        public override void Visit(GotoNode node)
        {
            AddInstruction("goto", node.Label.ToString(), "", "");
        }

        public override void Visit(LabeledStatementNode node)
        {
            AddInstruction("", "", "", "", node.Label.ToString());
            node.Stat.Visit(this);
        }
        
        public override void Visit(ForNode node)
        {
            var loopBeginLabel = GenerateTempLabel();
            var loopEndLabel = GenerateTempLabel();
            var begin = GenerateTACExpr(node.Begin);
            AddInstruction(AssignType.Assign.ToFriendlyString(), begin, "", node.Counter.Name);
            var end = GenerateTACExpr(node.End);
            var condVariable = GenerateTempName();
            AddInstruction(BinOpType.Greater.ToFriendlyString(), node.Counter.Name, end, condVariable, loopBeginLabel);
            AddInstruction("if goto", condVariable, loopEndLabel, "");
            node.Stat.Visit(this);
            AddInstruction(BinOpType.Plus.ToFriendlyString(), node.Counter.Name, "1", node.Counter.Name);
            AddInstruction("goto", loopBeginLabel, "", "");
            AddInstruction("", "", "", "", loopEndLabel);
        }

        private string GenerateTACExpr(ExprNode ex)
        {
            if (ex.GetType() == typeof(BinExprNode))
            {
                var bin = ex as BinExprNode;
                string tmp1 = GenerateTACExpr(bin.Left);
                string tmp2 = GenerateTACExpr(bin.Right);
                string tmp = GenerateTempName();
                AddInstruction(bin.OpType.ToFriendlyString(), tmp1, tmp2, tmp);
                return tmp;
            }
            if (ex.GetType() == typeof(UnoExprNode))
            {
                var uno = ex as UnoExprNode;
                string tmp1 = GenerateTACExpr(uno.Expr);
                string tmp = GenerateTempName();
                AddInstruction(uno.OpType.ToFriendlyString(), tmp1, "", tmp);
                return tmp;
            }
            if (ex is IdNode id)
            {
                return id.Name;
            }
            if (ex is FloatNumNode f)
            {
                return f.Num.ToString();
            }
            if (ex is BoolValNode b)
            {
                return b.Val.ToString();
            }
            if (ex is IntNumNode i)
            {
                return i.Num.ToString();
            }
            return "";
        }

        private void AddInstruction(string operation, string arg1, string arg2, string result, string label = "")
        {
            Instructions.Add(new TACInstruction(operation, arg1, arg2, result, label));
        }

        private static int tempVariableCounter = 0;
        public static string GenerateTempName()
        {
            return TEMP_NAME_PREFIX + (tempVariableCounter++);
        }

        private static int tempLabelCounter = 0;
        public static string GenerateTempLabel()
        {
            return TEMP_LABEL_PREFIX + (tempLabelCounter++);
        }
    }
}
