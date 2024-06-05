grammar SimpleLang;

prog:   stat+ ;

stat:   varDecl 
    |   assign 
    |   classDecl 
    |   expr 
    |   methodDecl 
    ;


classDecl: 'Class' ID '{' classVarDecl* '}' ;
classVarDecl: type ID ('[' dimensions ']')? ';' ;
varDecl: type ID ('[' dimensions ']')? ('=' expr)? ';' ;
assign: varReference ('[' indexList ']')? '=' expr ';' ;

methodDecl: type ID '(' argList? ')' block ;
type:   'int' | 'real' | 'string' | ID ;


dimensions:   INT (',' INT)* ;
indexList: expr (',' expr)* ;

argList: type ID (',' type ID)* ;

block: '{' stat* '}' ;


expr:   expr op=('*'|'/') expr  # MulDiv
    |   expr op=('+'|'-') expr  # AddSub
    |   INT                     # Int
    |   REAL                    # Real
    |   STRING                  # Str
    |   varReference            # VarRefLabel         
    |   '(' expr ')'            # Parens
    ;

varReference: ID ('.' ID)* ('[' indexList ']')* ;

ID:     [a-zA-Z_][a-zA-Z_0-9]* ;
INT:    [0-9]+ ;
REAL:   [0-9]+'.'[0-9]+ ;
STRING: '"' .*? '"' ;

WS:     [ \t\r\n]+ -> skip ;
