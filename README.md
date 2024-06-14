A simple language interpreter demonstrating the use of ANTLR G4 file and C# Visitor pattern to build a simple language interpreter.

Version:<br>
Targets: .NET (Core) 8<br>
C#: 12<br>
ANTLR: 4.13.<br>
Java: Version 22.0.1 (2024)<br>

Examine the .g4 for specifics, but basically the language allows simple types (int, real, string) and a composite type (Class), where
the members are simple types and accessed with the standard dot notation.

Output is written as file interpretorOutput.txt to the user's documentation folder.

The standard binary operators +, -, /, * are implemented along with parens.

Some samples that were tested:

Sample #1:<br>
int foo = 45; <br> 
string x = ""Diamond Age"";<br> 
real bar = (77 - 5 + (5*5)) / 25;<br> 
int wiz = foo + bar + 25;<br>

Sample #2:<br>
int bar = 50*23/4;<br> 
Class myClass1{int x; real y; string z; };<br>
myClass1 fooClass;<br>
fooClass.x = 47;<br>
int foo= fooClass.x;<br>

Generation of Visitor pattern C# code:<br>
java -jar antlr-4.13.1-complete.jar -Dlanguage=CSharp -visitor -o Generated SimpleLang.g4<br>

Workflow:<br>
The grammar/lexer .g4 is within the Grammar folder, and generated files are created in the Generated folder under Grammar.
When satisfied with the results, copy the .cs Generated files up to the top-level Generated folder for building/debugging/running.
Following the ANTLR recommendations, the generated files are *never* modified. Rather the methods are overwritten in the SimpleLangCustomVisitor.cs file.

The console output is redirected to a interpreterOutput.txt file in the user's documentation foldere. 

This is ongoing. Next steps: <br>
A. Adding language error handling<br>
B. Adding Test project<br>

Enjoy!




