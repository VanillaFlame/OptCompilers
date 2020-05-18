%{
    public BlockNode root;
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
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

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END FOR INTERVAL WHILE GOTO IF ELSE WRITE WRITE WRITELN READ ASSIGN SEMICOLON LBRACKET RBRACKET
%token MULTIPLICATE DIVIDE PLUS MINUS LESSTHAN GREATERTHAN LESSOREQUAL GREATEROREQUAL EQUAL NOTEQUAL AND OR COMMA NOT COLON
%token INT BOOL FLOAT
%token <iVal> INUM 
%token <dVal> RNUM
%token <bVal> BNUM 
%token <sVal> ID

%type <eVal> expr ident bool_t bool_f arithm_expr arithm_term unar_term factor
%type <stVal> assign statement for while func_call if goto labeled_statement declaration
%type <blVal> stlist block
%type <lNode> exprlist 
%type <idList> idlist

%%

progr		: block { root = $1; }
			;

stlist		: statement 
				{ 
					$$ = new BlockNode($1); 
				}
			| stlist statement 
				{ 
					$1.Add($2); 
					$$ = $1; 
				}
			;

exprlist	: expr
				{ 
					$$ = new ListExprNode($1);
				}
			| exprlist COMMA expr
				{ 
					$1.Add($3); 
					$$ = $1; 
				}
			;

statement	: declaration SEMICOLON		{ $$ = $1; }
			| assign SEMICOLON	{ $$ = $1; }
			| block SEMICOLON	{ $$ = $1; }
			| for				{ $$ = $1; }
			| while				{ $$ = $1; }
			| goto SEMICOLON	{ $$ = $1; }
			| labeled_statement			{ $$ = $1; }
			| if				{ $$ = $1; }
			| func_call SEMICOLON	{ $$ = $1; }
		    ;

declaration	: INT idlist { $$ = new DeclarationNode($2 as List<IdNode>, VariableType.Int); }
			| BOOL idlist { $$ = new DeclarationNode($2 as List<IdNode>, VariableType.Bool); }
			| FLOAT idlist { $$ = new DeclarationNode($2 as List<IdNode>, VariableType.Float); }
			;

idlist		: ident
				{ 
					$$ = new List<IdNode>();
					$$.Add($1 as IdNode);
				}
			| idlist COMMA ident
				{ 
					$1.Add($3 as IdNode); 
					$$ = $1; 
				}
			;

ident 		: ID { $$ = new IdNode($1); }	
			;
	
assign	 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
			;

expr		: expr OR bool_t { $$ = new BinExprNode($1, $3, BinOpType.Or); }
			| bool_t { $$ = $1; }
			;

bool_t		: bool_t AND bool_f { $$ = new BinExprNode($1, $3, BinOpType.And); }
			| bool_f { $$ = $1; }
			;

bool_f		: bool_f LESSTHAN arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.Less); }
			| bool_f GREATERTHAN arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.Greater); }
			| bool_f LESSOREQUAL arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.LessOrEqual); }
			| bool_f GREATEROREQUAL arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.GreaterOrEqual); }
			| bool_f EQUAL arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.Equal); }
			| bool_f NOTEQUAL arithm_expr { $$ = new BinExprNode($1, $3, BinOpType.NotEqual); }
			| arithm_expr { $$ = $1; }
			;

arithm_expr	: arithm_expr PLUS arithm_term { $$ = new BinExprNode($1, $3, BinOpType.Plus); }
			| arithm_expr MINUS arithm_term { $$ = new BinExprNode($1, $3, BinOpType.Minus); }
			| arithm_term { $$ = $1; }
			;

arithm_term	: arithm_term MULTIPLICATE unar_term { $$ = new BinExprNode($1, $3, BinOpType.Prod); }
			| arithm_term DIVIDE unar_term { $$ = new BinExprNode($1, $3, BinOpType.Div); }
			| unar_term { $$ = $1; }
			;

unar_term	: NOT factor { $$ = new UnoExprNode($2, UnoOpType.Not); }
			| MINUS factor { $$ = new UnoExprNode($2, UnoOpType.Minus); }
			| factor { $$ = $1; }
			;

factor		: INUM { $$ = new IntNumNode($1); }
			| RNUM { $$ = new FloatNumNode($1); }
			| BNUM { $$ = new BoolValNode($1); }
			| ident { $$ = $1; }
			| LBRACKET expr RBRACKET { $$ = $2; }
			;

block		: BEGIN stlist END { $$ = $2; }
			;

for			: FOR ident ASSIGN expr INTERVAL expr block { $$ = new ForNode($2 as IdNode, $4, $6, $7); }
			;

while		: WHILE expr block { $$ = new WhileNode($2, $3); }
			;

goto		: GOTO INUM { $$ = new GotoNode($2); }
			;

labeled_statement : INUM COLON statement { $$ = new LabeledStatementNode($1, $3); }
			;


if			: IF expr block { $$ = new IfNode($2, $3); }
			| IF expr block ELSE block { $$ = new IfNode($2, $3, $5); }
			;

func_call	: WRITE LBRACKET exprlist RBRACKET { $$ = new WriteNode($3 as ListExprNode, false); }
			| WRITELN LBRACKET exprlist RBRACKET { $$ = new WriteNode($3 as ListExprNode, true); }
			| READ LBRACKET ident RBRACKET { $$ = new ReadNode($3 as IdNode); }
			;
	
%%
