using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;


namespace SimpleLangInterpreter
{
    static class SimpleLangInterpreter
    {
        public static void Run(string programPath, string logPath, string debugPath)
        {
            // Test cases
            var input1 = "int foo = (45 - 5 + (5*4))/20; int bar = foo+20;"; // answers: foo=3 bar=23
            var input2 = @"int foo = (45 - 5 + (5*4))/20;  
                real bar = foo+20.1
                ;";// answers: foo=3 bar=23.1

            // String, int, real, parens foo=45, x='wassup' bar=3/88 wiz=74
            var input3 = @"int foo = 45;  string x = ""SnowCrash""; 
                real bar = (77 - 5 + (5*5)) / 25; 
                int wiz = foo + bar + 25; 
                "; // Example input

            string input9 = "int bar = 66; class MyClass1 { int x; string y; }; MyClass1 obj";

            var input10 = @" int bar = 50*23/4; 
                Class myClass1{int x; real y; string z; };
                myClass1 fooClass;
                fooClass.x = 47;
                int foo= fooClass.x;
            ";

            var input11 = @" int bar = 50*23/4; 
                Class myClass1{int a; real b; string c; };
                Class myClass2{int x; real b; myClass1 c1; };

                myClass1 C1;
                C1.a = 47;
                C1.b = 3.14159;
                C1.c = ""Diamond Age"";

                int foo= C1.a;

                myClass2 C2;
                C2.x = foo;
                C2.c1 = C1;
            ";

            // Error inputs
            var input100 = @" int bar = 50*23/4; 
                Class myClass1{int x; real y; string z; ;
                myClass1 fooClass;
                fooClass.x = 47;
                int foo= fooClass.x;
            ";


            try
            {
                string testInput = input2;
                Console.Write($"Input: [{testInput}]");
                var inputStream = new AntlrInputStream(testInput);
                var lexer = new SimpleLangLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new SimpleLangParser(commonTokenStream);

                var errorListener = new CustomErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);

                var context = parser.prog();

                var visitor = new SimpleLangCustomVisitor();
                var result = visitor.Visit(context);

                foreach (var error in errorListener.Errors)
                {
                    visitor.Errors.Add(error);
                }

                GenerateOutputFile(visitor.Lines, visitor.Errors);

                // Example to print variable values
                Console.WriteLine($"Writing {visitor.Variables.Count()} Variables:");
                foreach (var variable in visitor.Variables)
                {
                    Console.WriteLine($"{variable.Key} ({variable.Value.Type}) = {variable}");
                }

            }
            catch (Exception ex)
            {
                Logging.LogIt($"Exception={ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Write an output file listing the code and any errors 
        /// that occurred.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="errors"></param>
        private static void GenerateOutputFile(List<string> lines, List<string> errors)
        {
            // Get the path to the user's Documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Define the output file path
            string outputFilePath = Path.Combine(documentsPath, "CodeOutput.txt");

            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("Parsed Lines:");
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }

                writer.WriteLine("\nErrors:");
                foreach (var error in errors)
                {
                    writer.WriteLine(error);
                }
            }
        }



    }
}
