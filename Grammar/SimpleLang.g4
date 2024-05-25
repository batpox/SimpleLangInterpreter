grammar SimpleLang;

prog:   stat+ ;

stat:   varDecl
    |   assign
    |   expr
    |   methodDecl
    |   classDecl
    ;

varDecl: type ID ('=' expr)? ';' ;

assign: varRef '=' expr ';' ;

type:   'int'
    |   'real'
    |   'string'
    |   'int' '[' dimensions ']'
    |   'real' '[' dimensions ']'
    |   'string' '[' dimensions ']'
    |   ID // for class types
    ;

dimensions:   INT (',' INT)* ;

methodDecl: type ID '(' argList? ')' block ;

argList: type ID (',' type ID)* ;

block: '{' stat* '}' ;

classDecl: 'class' ID '{' classVarDecl* '}' ;

classVarDecl: type ID ';' ;

expr:   expr ('*'|'/') expr
    |   expr ('+'|'-') expr
    |   varRef
    |   INT
    |   REAL
    |   STRING
    |   varRef '[' expr ']'
    |   '(' expr ')'
    ;

varRef: ID ('.' ID)* ;

ID:     [a-zA-Z_][a-zA-Z_0-9]* ;
INT:    [0-9]+ ;
REAL:   [0-9]+'.'[0-9]+ ;
STRING: '"' .*? '"' ;

WS:     [ \t\r\n]+ -> skip ;
