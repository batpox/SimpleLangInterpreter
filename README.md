A simple language interpreter demonstrating the use of ANTLR G4 file and C# Visitor pattern.

Versions: ANTLR 4.13.1
Java: Version 8 Update 411 (2024)

Generation of pattern:
java -jar antlr-4.13.1-complete.jar -Dlanguage=CSharp -visitor -o Generated SimpleLang.g4

Workflow:
The grammar/lexer .g4 is within the Grammar folder, and generated files are created in the Generated folder under Grammar.
When satisfied with the results, copy the .cs Generated files up to the top-level Generated folder for building/debugging/running.
Following the ANTLR recommendations, the generated files are *never* modified. Rather the methods are overwritten in the SimpleLangCustomVisitor.cs file.

The output is written to a c:\temp\interpreterOutput.txt (in main of program.cs).



