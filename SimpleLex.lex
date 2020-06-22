%using SimpleParser;
%using QUT.Gppg;
%using System.Linq;

%namespace SimpleScanner

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}* 
BOOLVAL	"true"|"false"

%%

{INTNUM} { 
  yylval.iVal = int.Parse(yytext); 
  return (int)Tokens.INUM; 
}

{REALNUM} { 
  yylval.dVal = double.Parse(yytext, CultureInfo.InvariantCulture); 
  return (int)Tokens.RNUM;
}

{BOOLVAL}  { 
  yylval.bVal = bool.Parse(yytext); 
  return (int)Tokens.BNUM;
}

{ID}  { 
  int res = ScannerHelper.GetIDToken(yytext);
  if (res == (int)Tokens.ID)
	yylval.sVal = yytext;
  return res;
}


"=" { return (int)Tokens.ASSIGN; }
";" { return (int)Tokens.SEMICOLON; }
"{" { return (int)Tokens.BEGIN; }
"}" { return (int)Tokens.END; }
"<" { return (int)Tokens.LESSTHAN; }
">" { return (int)Tokens.GREATERTHAN; }
"<=" { return (int)Tokens.LESSOREQUAL; }
">=" { return (int)Tokens.GREATEROREQUAL; }
"==" { return (int)Tokens.EQUAL; }
"!=" { return (int)Tokens.NOTEQUAL; }
".." { return (int)Tokens.INTERVAL; }
","	{ return (int)Tokens.COMMA;	}
"+" { return (int)Tokens.PLUS; }
"-" { return (int)Tokens.MINUS; }
"*" { return (int)Tokens.MULTIPLICATE; }
"/" { return (int)Tokens.DIVIDE; }
"&" { return (int)Tokens.AND; }
"|" { return (int)Tokens.OR; }
"!" { return (int)Tokens.NOT; }
"(" { return (int)Tokens.LBRACKET; }
")" { return (int)Tokens.RBRACKET; }
":" { return (int)Tokens.COLON; }

[^ \r\n] {
	LexError();
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

public override void yyerror(string format, params object[] args) // обработка синтаксических ошибок
{
  var ww = args.Skip(1).Cast<string>().ToArray();
  string errorMsg = string.Format("({0},{1}): Встречено {2}, а ожидалось {3}", yyline, yycol, args[0], string.Join(" или ", ww));
  throw new SyntaxException(errorMsg);
}

public void LexError()
{
  string errorMsg = string.Format("({0},{1}): Неизвестный символ {2}", yyline, yycol, yytext);
  throw new LexException(errorMsg);
}

class ScannerHelper 
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper() 
  {
    keywords = new Dictionary<string,int>();
    keywords.Add("for",(int)Tokens.FOR);
	keywords.Add("while",(int)Tokens.WHILE);
	keywords.Add("goto",(int)Tokens.GOTO);
	keywords.Add("if",(int)Tokens.IF);
	keywords.Add("else",(int)Tokens.ELSE);
	keywords.Add("write",(int)Tokens.WRITE);
	keywords.Add("writeln",(int)Tokens.WRITELN);
	keywords.Add("read",(int)Tokens.READ);
	keywords.Add("int",(int)Tokens.INT);
	keywords.Add("bool",(int)Tokens.BOOL);
	keywords.Add("float",(int)Tokens.FLOAT);
  }
  public static int GetIDToken(string s)
  {
	if (keywords.ContainsKey(s.ToLower()))
	  return keywords[s];
	else
      return (int)Tokens.ID;
  }
  
}
