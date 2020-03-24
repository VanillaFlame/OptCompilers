// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  DESKTOP-0FGGHDS
// DateTime: 3/24/2020 2:35:18 PM
// UserName: annad
// Input file <SimpleYacc.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using ProgramTree;

namespace SimpleParser
{
public enum Tokens {
    error=1,EOF=2,BEGIN=3,END=4,FOR=5,INTERVAL=6,
    WHILE=7,GOTO=8,IF=9,ELSE=10,WRITE=11,WRITELN=12,
    READ=13,ASSIGN=14,SEMICOLON=15,LBRACKET=16,RBRACKET=17,MULTIPLICATE=18,
    DIVIDE=19,PLUS=20,MINUS=21,LESSTHAN=22,GREATERTHAN=23,LESSOREQUAL=24,
    GREATEROREQUAL=25,EQUAL=26,NOTEQUAL=27,AND=28,OR=29,COMMA=30,
    NOT=31,COLON=32,INT=33,BOOL=34,FLOAT=35,INUM=36,
    RNUM=37,BNUM=38,ID=39};

public struct ValueType
{ 
			public double dVal; 
			public int iVal; 
			public bool bVal;
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
			public ListExprNode lNode;
			public List<IdNode> idList;
       }
// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from SimpleYacc.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public BlockNode root; // �������� ���� ��������������� ������ 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from SimpleYacc.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[58];
  private static State[] states = new State[112];
  private static string[] nonTerms = new string[] {
      "expr", "ident", "bool_t", "bool_f", "arithm_expr", "arithm_term", "unar_term", 
      "factor", "assign", "statement", "for", "while", "func_call", "if", "goto", 
      "labeled_statement", "declaration", "stlist", "block", "exprlist", "idlist", 
      "progr", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{3,4},new int[]{-22,1,-19,3});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{33,10,34,16,35,18,39,14,3,4,5,69,7,77,8,82,36,85,9,89,11,96,12,103,13,107},new int[]{-18,5,-10,111,-17,8,-9,20,-2,22,-19,66,-11,68,-12,76,-15,80,-16,84,-14,88,-13,94});
    states[5] = new State(new int[]{4,6,33,10,34,16,35,18,39,14,3,4,5,69,7,77,8,82,36,85,9,89,11,96,12,103,13,107},new int[]{-10,7,-17,8,-9,20,-2,22,-19,66,-11,68,-12,76,-15,80,-16,84,-14,88,-13,94});
    states[6] = new State(-48);
    states[7] = new State(-4);
    states[8] = new State(new int[]{15,9});
    states[9] = new State(-7);
    states[10] = new State(new int[]{39,14},new int[]{-21,11,-2,15});
    states[11] = new State(new int[]{30,12,15,-16});
    states[12] = new State(new int[]{39,14},new int[]{-2,13});
    states[13] = new State(-20);
    states[14] = new State(-21);
    states[15] = new State(-19);
    states[16] = new State(new int[]{39,14},new int[]{-21,17,-2,15});
    states[17] = new State(new int[]{30,12,15,-17});
    states[18] = new State(new int[]{39,14},new int[]{-21,19,-2,15});
    states[19] = new State(new int[]{30,12,15,-18});
    states[20] = new State(new int[]{15,21});
    states[21] = new State(-8);
    states[22] = new State(new int[]{14,23});
    states[23] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,24,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[24] = new State(new int[]{29,25,15,-22});
    states[25] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-3,26,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[26] = new State(new int[]{28,27,29,-23,15,-23,17,-23,6,-23,3,-23,30,-23});
    states[27] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-4,28,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[28] = new State(new int[]{22,29,23,46,24,57,25,59,26,61,27,63,28,-25,29,-25,15,-25,17,-25,6,-25,3,-25,30,-25});
    states[29] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,30,-6,56,-7,55,-8,54,-2,40});
    states[30] = new State(new int[]{20,31,21,48,22,-27,23,-27,24,-27,25,-27,26,-27,27,-27,28,-27,29,-27,15,-27,17,-27,6,-27,3,-27,30,-27});
    states[31] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-6,32,-7,55,-8,54,-2,40});
    states[32] = new State(new int[]{18,33,19,50,20,-34,21,-34,22,-34,23,-34,24,-34,25,-34,26,-34,27,-34,28,-34,29,-34,15,-34,17,-34,6,-34,3,-34,30,-34});
    states[33] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-7,34,-8,54,-2,40});
    states[34] = new State(-37);
    states[35] = new State(new int[]{36,37,37,38,38,39,39,14,16,41},new int[]{-8,36,-2,40});
    states[36] = new State(-40);
    states[37] = new State(-43);
    states[38] = new State(-44);
    states[39] = new State(-45);
    states[40] = new State(-46);
    states[41] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,42,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[42] = new State(new int[]{17,43,29,25});
    states[43] = new State(-47);
    states[44] = new State(new int[]{28,27,29,-24,15,-24,17,-24,6,-24,3,-24,30,-24});
    states[45] = new State(new int[]{22,29,23,46,24,57,25,59,26,61,27,63,28,-26,29,-26,15,-26,17,-26,6,-26,3,-26,30,-26});
    states[46] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,47,-6,56,-7,55,-8,54,-2,40});
    states[47] = new State(new int[]{20,31,21,48,22,-28,23,-28,24,-28,25,-28,26,-28,27,-28,28,-28,29,-28,15,-28,17,-28,6,-28,3,-28,30,-28});
    states[48] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-6,49,-7,55,-8,54,-2,40});
    states[49] = new State(new int[]{18,33,19,50,20,-35,21,-35,22,-35,23,-35,24,-35,25,-35,26,-35,27,-35,28,-35,29,-35,15,-35,17,-35,6,-35,3,-35,30,-35});
    states[50] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-7,51,-8,54,-2,40});
    states[51] = new State(-38);
    states[52] = new State(new int[]{36,37,37,38,38,39,39,14,16,41},new int[]{-8,53,-2,40});
    states[53] = new State(-41);
    states[54] = new State(-42);
    states[55] = new State(-39);
    states[56] = new State(new int[]{18,33,19,50,20,-36,21,-36,22,-36,23,-36,24,-36,25,-36,26,-36,27,-36,28,-36,29,-36,15,-36,17,-36,6,-36,3,-36,30,-36});
    states[57] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,58,-6,56,-7,55,-8,54,-2,40});
    states[58] = new State(new int[]{20,31,21,48,22,-29,23,-29,24,-29,25,-29,26,-29,27,-29,28,-29,29,-29,15,-29,17,-29,6,-29,3,-29,30,-29});
    states[59] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,60,-6,56,-7,55,-8,54,-2,40});
    states[60] = new State(new int[]{20,31,21,48,22,-30,23,-30,24,-30,25,-30,26,-30,27,-30,28,-30,29,-30,15,-30,17,-30,6,-30,3,-30,30,-30});
    states[61] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,62,-6,56,-7,55,-8,54,-2,40});
    states[62] = new State(new int[]{20,31,21,48,22,-31,23,-31,24,-31,25,-31,26,-31,27,-31,28,-31,29,-31,15,-31,17,-31,6,-31,3,-31,30,-31});
    states[63] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-5,64,-6,56,-7,55,-8,54,-2,40});
    states[64] = new State(new int[]{20,31,21,48,22,-32,23,-32,24,-32,25,-32,26,-32,27,-32,28,-32,29,-32,15,-32,17,-32,6,-32,3,-32,30,-32});
    states[65] = new State(new int[]{20,31,21,48,22,-33,23,-33,24,-33,25,-33,26,-33,27,-33,28,-33,29,-33,15,-33,17,-33,6,-33,3,-33,30,-33});
    states[66] = new State(new int[]{15,67});
    states[67] = new State(-9);
    states[68] = new State(-10);
    states[69] = new State(new int[]{39,14},new int[]{-2,70});
    states[70] = new State(new int[]{14,71});
    states[71] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,72,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[72] = new State(new int[]{6,73,29,25});
    states[73] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,74,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[74] = new State(new int[]{29,25,3,4},new int[]{-19,75});
    states[75] = new State(-49);
    states[76] = new State(-11);
    states[77] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,78,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[78] = new State(new int[]{29,25,3,4},new int[]{-19,79});
    states[79] = new State(-50);
    states[80] = new State(new int[]{15,81});
    states[81] = new State(-12);
    states[82] = new State(new int[]{36,83});
    states[83] = new State(-51);
    states[84] = new State(-13);
    states[85] = new State(new int[]{32,86});
    states[86] = new State(new int[]{33,10,34,16,35,18,39,14,3,4,5,69,7,77,8,82,36,85,9,89,11,96,12,103,13,107},new int[]{-10,87,-17,8,-9,20,-2,22,-19,66,-11,68,-12,76,-15,80,-16,84,-14,88,-13,94});
    states[87] = new State(-52);
    states[88] = new State(-14);
    states[89] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,90,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[90] = new State(new int[]{29,25,3,4},new int[]{-19,91});
    states[91] = new State(new int[]{10,92,4,-53,33,-53,34,-53,35,-53,39,-53,3,-53,5,-53,7,-53,8,-53,36,-53,9,-53,11,-53,12,-53,13,-53});
    states[92] = new State(new int[]{3,4},new int[]{-19,93});
    states[93] = new State(-54);
    states[94] = new State(new int[]{15,95});
    states[95] = new State(-15);
    states[96] = new State(new int[]{16,97});
    states[97] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-20,98,-1,102,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[98] = new State(new int[]{17,99,30,100});
    states[99] = new State(-55);
    states[100] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-1,101,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[101] = new State(new int[]{29,25,17,-6,30,-6});
    states[102] = new State(new int[]{29,25,17,-5,30,-5});
    states[103] = new State(new int[]{16,104});
    states[104] = new State(new int[]{31,35,21,52,36,37,37,38,38,39,39,14,16,41},new int[]{-20,105,-1,102,-3,44,-4,45,-5,65,-6,56,-7,55,-8,54,-2,40});
    states[105] = new State(new int[]{17,106,30,100});
    states[106] = new State(-56);
    states[107] = new State(new int[]{16,108});
    states[108] = new State(new int[]{39,14},new int[]{-2,109});
    states[109] = new State(new int[]{17,110});
    states[110] = new State(-57);
    states[111] = new State(-3);

    rules[1] = new Rule(-23, new int[]{-22,2});
    rules[2] = new Rule(-22, new int[]{-19});
    rules[3] = new Rule(-18, new int[]{-10});
    rules[4] = new Rule(-18, new int[]{-18,-10});
    rules[5] = new Rule(-20, new int[]{-1});
    rules[6] = new Rule(-20, new int[]{-20,30,-1});
    rules[7] = new Rule(-10, new int[]{-17,15});
    rules[8] = new Rule(-10, new int[]{-9,15});
    rules[9] = new Rule(-10, new int[]{-19,15});
    rules[10] = new Rule(-10, new int[]{-11});
    rules[11] = new Rule(-10, new int[]{-12});
    rules[12] = new Rule(-10, new int[]{-15,15});
    rules[13] = new Rule(-10, new int[]{-16});
    rules[14] = new Rule(-10, new int[]{-14});
    rules[15] = new Rule(-10, new int[]{-13,15});
    rules[16] = new Rule(-17, new int[]{33,-21});
    rules[17] = new Rule(-17, new int[]{34,-21});
    rules[18] = new Rule(-17, new int[]{35,-21});
    rules[19] = new Rule(-21, new int[]{-2});
    rules[20] = new Rule(-21, new int[]{-21,30,-2});
    rules[21] = new Rule(-2, new int[]{39});
    rules[22] = new Rule(-9, new int[]{-2,14,-1});
    rules[23] = new Rule(-1, new int[]{-1,29,-3});
    rules[24] = new Rule(-1, new int[]{-3});
    rules[25] = new Rule(-3, new int[]{-3,28,-4});
    rules[26] = new Rule(-3, new int[]{-4});
    rules[27] = new Rule(-4, new int[]{-4,22,-5});
    rules[28] = new Rule(-4, new int[]{-4,23,-5});
    rules[29] = new Rule(-4, new int[]{-4,24,-5});
    rules[30] = new Rule(-4, new int[]{-4,25,-5});
    rules[31] = new Rule(-4, new int[]{-4,26,-5});
    rules[32] = new Rule(-4, new int[]{-4,27,-5});
    rules[33] = new Rule(-4, new int[]{-5});
    rules[34] = new Rule(-5, new int[]{-5,20,-6});
    rules[35] = new Rule(-5, new int[]{-5,21,-6});
    rules[36] = new Rule(-5, new int[]{-6});
    rules[37] = new Rule(-6, new int[]{-6,18,-7});
    rules[38] = new Rule(-6, new int[]{-6,19,-7});
    rules[39] = new Rule(-6, new int[]{-7});
    rules[40] = new Rule(-7, new int[]{31,-8});
    rules[41] = new Rule(-7, new int[]{21,-8});
    rules[42] = new Rule(-7, new int[]{-8});
    rules[43] = new Rule(-8, new int[]{36});
    rules[44] = new Rule(-8, new int[]{37});
    rules[45] = new Rule(-8, new int[]{38});
    rules[46] = new Rule(-8, new int[]{-2});
    rules[47] = new Rule(-8, new int[]{16,-1,17});
    rules[48] = new Rule(-19, new int[]{3,-18,4});
    rules[49] = new Rule(-11, new int[]{5,-2,14,-1,6,-1,-19});
    rules[50] = new Rule(-12, new int[]{7,-1,-19});
    rules[51] = new Rule(-15, new int[]{8,36});
    rules[52] = new Rule(-16, new int[]{36,32,-10});
    rules[53] = new Rule(-14, new int[]{9,-1,-19});
    rules[54] = new Rule(-14, new int[]{9,-1,-19,10,-19});
    rules[55] = new Rule(-13, new int[]{11,16,-20,17});
    rules[56] = new Rule(-13, new int[]{12,16,-20,17});
    rules[57] = new Rule(-13, new int[]{13,16,-2,17});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
    switch (action)
    {
      case 2: // progr -> block
{ root = ValueStack[ValueStack.Depth-1].blVal; }
        break;
      case 3: // stlist -> statement
{ 
					CurrentSemanticValue.blVal = new BlockNode(ValueStack[ValueStack.Depth-1].stVal); 
				}
        break;
      case 4: // stlist -> stlist, statement
{ 
					ValueStack[ValueStack.Depth-2].blVal.Add(ValueStack[ValueStack.Depth-1].stVal); 
					CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-2].blVal; 
				}
        break;
      case 5: // exprlist -> expr
{ 
					CurrentSemanticValue.lNode = new ListExprNode(ValueStack[ValueStack.Depth-1].eVal);
				}
        break;
      case 6: // exprlist -> exprlist, COMMA, expr
{ 
					ValueStack[ValueStack.Depth-3].lNode.Add(ValueStack[ValueStack.Depth-1].eVal); 
					CurrentSemanticValue.lNode = ValueStack[ValueStack.Depth-3].lNode; 
				}
        break;
      case 7: // statement -> declaration, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].stVal; }
        break;
      case 8: // statement -> assign, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].stVal; }
        break;
      case 9: // statement -> block, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].blVal; }
        break;
      case 10: // statement -> for
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 11: // statement -> while
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 12: // statement -> goto, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].stVal; }
        break;
      case 13: // statement -> labeled_statement
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 14: // statement -> if
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 15: // statement -> func_call, SEMICOLON
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-2].stVal; }
        break;
      case 16: // declaration -> INT, idlist
{ CurrentSemanticValue.stVal = new DeclarationNode(ValueStack[ValueStack.Depth-1].idList as List<IdNode>, VariableType.Int); }
        break;
      case 17: // declaration -> BOOL, idlist
{ CurrentSemanticValue.stVal = new DeclarationNode(ValueStack[ValueStack.Depth-1].idList as List<IdNode>, VariableType.Bool); }
        break;
      case 18: // declaration -> FLOAT, idlist
{ CurrentSemanticValue.stVal = new DeclarationNode(ValueStack[ValueStack.Depth-1].idList as List<IdNode>, VariableType.Float); }
        break;
      case 19: // idlist -> ident
{ 
					CurrentSemanticValue.idList = new List<IdNode>();
					CurrentSemanticValue.idList.Add(ValueStack[ValueStack.Depth-1].eVal as IdNode);
				}
        break;
      case 20: // idlist -> idlist, COMMA, ident
{ 
					ValueStack[ValueStack.Depth-3].idList.Add(ValueStack[ValueStack.Depth-1].eVal as IdNode); 
					CurrentSemanticValue.idList = ValueStack[ValueStack.Depth-3].idList; 
				}
        break;
      case 21: // ident -> ID
{ CurrentSemanticValue.eVal = new IdNode(ValueStack[ValueStack.Depth-1].sVal); }
        break;
      case 22: // assign -> ident, ASSIGN, expr
{ CurrentSemanticValue.stVal = new AssignNode(ValueStack[ValueStack.Depth-3].eVal as IdNode, ValueStack[ValueStack.Depth-1].eVal); }
        break;
      case 23: // expr -> expr, OR, bool_t
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Or); }
        break;
      case 24: // expr -> bool_t
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 25: // bool_t -> bool_t, AND, bool_f
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.And); }
        break;
      case 26: // bool_t -> bool_f
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 27: // bool_f -> bool_f, LESSTHAN, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Less); }
        break;
      case 28: // bool_f -> bool_f, GREATERTHAN, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Greater); }
        break;
      case 29: // bool_f -> bool_f, LESSOREQUAL, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.LessOrEqual); }
        break;
      case 30: // bool_f -> bool_f, GREATEROREQUAL, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.GreaterOrEqual); }
        break;
      case 31: // bool_f -> bool_f, EQUAL, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Equal); }
        break;
      case 32: // bool_f -> bool_f, NOTEQUAL, arithm_expr
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.NotEqual); }
        break;
      case 33: // bool_f -> arithm_expr
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 34: // arithm_expr -> arithm_expr, PLUS, arithm_term
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Plus); }
        break;
      case 35: // arithm_expr -> arithm_expr, MINUS, arithm_term
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Minus); }
        break;
      case 36: // arithm_expr -> arithm_term
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 37: // arithm_term -> arithm_term, MULTIPLICATE, unar_term
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Prod); }
        break;
      case 38: // arithm_term -> arithm_term, DIVIDE, unar_term
{ CurrentSemanticValue.eVal = new BinExprNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].eVal, BinOpType.Div); }
        break;
      case 39: // arithm_term -> unar_term
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 40: // unar_term -> NOT, factor
{ CurrentSemanticValue.eVal = new UnoExprNode(ValueStack[ValueStack.Depth-1].eVal, UnoOpType.Not); }
        break;
      case 41: // unar_term -> MINUS, factor
{ CurrentSemanticValue.eVal = new UnoExprNode(ValueStack[ValueStack.Depth-1].eVal, UnoOpType.Minus); }
        break;
      case 42: // unar_term -> factor
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 43: // factor -> INUM
{ CurrentSemanticValue.eVal = new IntNumNode(ValueStack[ValueStack.Depth-1].iVal); }
        break;
      case 44: // factor -> RNUM
{ CurrentSemanticValue.eVal = new FloatNumNode(ValueStack[ValueStack.Depth-1].dVal); }
        break;
      case 45: // factor -> BNUM
{ CurrentSemanticValue.eVal = new BoolValNode(ValueStack[ValueStack.Depth-1].bVal); }
        break;
      case 46: // factor -> ident
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 47: // factor -> LBRACKET, expr, RBRACKET
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-2].eVal; }
        break;
      case 48: // block -> BEGIN, stlist, END
{ CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-2].blVal; }
        break;
      case 49: // for -> FOR, ident, ASSIGN, expr, INTERVAL, expr, block
{ CurrentSemanticValue.stVal = new ForNode(ValueStack[ValueStack.Depth-6].eVal as IdNode, ValueStack[ValueStack.Depth-4].eVal, ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].blVal); }
        break;
      case 50: // while -> WHILE, expr, block
{ CurrentSemanticValue.stVal = new WhileNode(ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].blVal); }
        break;
      case 51: // goto -> GOTO, INUM
{ CurrentSemanticValue.stVal = new GotoNode(ValueStack[ValueStack.Depth-1].iVal); }
        break;
      case 52: // labeled_statement -> INUM, COLON, statement
{ CurrentSemanticValue.stVal = new LabeledStatementNode(ValueStack[ValueStack.Depth-3].iVal, ValueStack[ValueStack.Depth-1].stVal); }
        break;
      case 53: // if -> IF, expr, block
{ CurrentSemanticValue.stVal = new IfNode(ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].blVal); }
        break;
      case 54: // if -> IF, expr, block, ELSE, block
{ CurrentSemanticValue.stVal = new IfNode(ValueStack[ValueStack.Depth-4].eVal, ValueStack[ValueStack.Depth-3].blVal, ValueStack[ValueStack.Depth-1].blVal); }
        break;
      case 55: // func_call -> WRITE, LBRACKET, exprlist, RBRACKET
{ CurrentSemanticValue.stVal = new WriteNode(ValueStack[ValueStack.Depth-2].lNode as ListExprNode, false); }
        break;
      case 56: // func_call -> WRITELN, LBRACKET, exprlist, RBRACKET
{ CurrentSemanticValue.stVal = new WriteNode(ValueStack[ValueStack.Depth-2].lNode as ListExprNode, true); }
        break;
      case 57: // func_call -> READ, LBRACKET, ident, RBRACKET
{ CurrentSemanticValue.stVal = new ReadNode(ValueStack[ValueStack.Depth-2].eVal as IdNode); }
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
