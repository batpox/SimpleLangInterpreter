A simple language interpreter demonstrating the use of ANTLR G4 file and C# Visitor pattern to build a simple language interpreter.

Examine the .g4 for specifics, but basically the language allows simple types (int, real, string) and a composite type (Class), where
the members are simple types and accessed with the standard dot notation.

The standard binary operators +, -, /, * are implemented along with parens.

Some samples that were tested:

Sample #1:
int foo = 45;  
string x = ""Diamond Age""; 
real bar = (77 - 5 + (5*5)) / 25; 
int wiz = foo + bar + 25; 

Sample #2:
int bar = 50*23/4; 
Class myClass1{int x; real y; string z; };
myClass1 fooClass;
fooClass.x = 47;
int foo= fooClass.x;

Versions: ANTLR 4.13.1
Java: Version 8 Update 411 (2024)

Generation of Visitor pattern C# code.
java -jar antlr-4.13.1-complete.jar -Dlanguage=CSharp -visitor -o Generated SimpleLang.g4
The java is version 22.0.1

Workflow:
The grammar/lexer .g4 is within the Grammar folder, and generated files are created in the Generated folder under Grammar.
When satisfied with the results, copy the .cs Generated files up to the top-level Generated folder for building/debugging/running.
Following the ANTLR recommendations, the generated files are *never* modified. Rather the methods are overwritten in the SimpleLangCustomVisitor.cs file.

The output is written to a c:\temp\interpreterOutput.txt (in main of program.cs).

This is ongoing. Next steps: 
A. Adding complex type
B. Language for motion description

Enjoy!




