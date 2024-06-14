grammar SimpleLang;

prog:   stat+ ;

stat:   varDecl 
    |   assign 
    |   classDecl 
    |   expr 
    |   methodDecl 
    |   ifStat
    ;

classDecl: 'Class' ID '{' classVarDecl* '}' ';' ;
classVarDecl: type ID ('[' dimensions ']')? ';' ;
varDecl: type ID ('[' dimensions ']')? ('=' expr)? ';' ;
assign: varReference ('[' indexList ']')? '=' expr ';' ;

methodDecl: type ID '(' argList? ')' block ;
type:   'int' | 'real' | 'string' | 'BOOL' | ID ;

dimensions:   INT (',' INT)* ;
indexList: expr (',' expr)* ;

argList: type ID (',' type ID)* ;

block: '{' stat* '}' ;

ifStat: 'if' '(' logicalExpr ')' block ( 'else' block )? ;

expr:   expr op=('+'|'-') expr  # AddSub
    |   expr op=('*'|'/') expr  # MulDiv
    |   '!' expr                # Not
    |   INT                     # Int
    |   REAL                    # Real
    |   STRING                  # Str
    |   BOOL                    # Bool
    |   varReference            # VarRef
    |   '(' expr ')'            # Parens
    ;

logicalExpr: expr compOp expr # Comparison
    ;

compOp: '>' | '<' | '>=' | '<=' | '==' | '!=' ;

varReference: ID ('.' ID)* ('[' indexList ']')* ;

ID:     [a-zA-Z_][a-zA-Z_0-9]* ;
INT:    [0-9]+ ;
REAL:   [0-9]+'.'[0-9]+ ;
STRING: '"' .*? '"' ;
BOOL:   'true' | 'false' ;

WS:     [ \t\r\n]+ -> skip ;
