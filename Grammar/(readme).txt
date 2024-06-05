When the grammar changes, we must modify the .g4 and regenerate all the code
located in the folder 'Generated'

This is done with a command line like this:

Move to where the Grammar folder is and run ANTLR (which was placed within the Grammar folder)

cd c:\Users\dan_h\source\repos\SimpleLangInterpreter\Grammar

java -jar antlr-4.13.1-complete.jar -Dlanguage=CSharp -visitor -o Generated SimpleLang.g4

And then move the generated code up the our solutions 'Generated' folder using File Explorer, 
thus replacing the files under Generated. Of course, you can organize files as you wish.
I just found this to be the best for me.

The change to .g4 may change what the Visitor code needs to be, so then alter the 'Custom'
visitor code, such as 'SimpleLangCustomVisitor.cs'

*NEVER* manually alter the generated code!

java -version => java version "1.8.0_411"
Java(TM) SE Runtime Environment (build 1.8.0_411-b09)


