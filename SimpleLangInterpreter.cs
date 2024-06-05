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
        public static void Run(string[] args)
        {

            var input1 = "int foo = (45 - 5 + (5*4))/20; int bar = foo+20;"; // answers: foo=3 bar=23
            var input2 = @"int foo = (45 - 5 + (5*4))/20;  
                real bar = foo+20.1
                ;";// answers: foo=3 bar=23.1

            // String, int, real, parens foo=45, x='wassup' bar=3/88 wiz=74
            var input3 = @"int foo = 45;  string x = ""wassup""; 
                real bar = (77 - 5 + (5*5)) / 25; 
                int wiz = foo + bar + 25; 
                "; // Example input

            var input10 = @"Class myClass1{int x; real y; string z; };
                myClass1 fooClass;
                int foo= fooClass.x;
            ";

            string input9 = "int bar = 66; class MyClass1 { int x; string y; }; MyClass1 obj";

            try
            {
                var inputStream = new AntlrInputStream(input10);
                var lexer = new SimpleLangLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new SimpleLangParser(commonTokenStream);
                var context = parser.prog();

                var visitor = new SimpleLangCustomVisitor();
                var result = visitor.Visit(context);

                // Example to print variable values
                foreach (var variable in visitor.Variables)
                {
                    if (variable.Value.Type.StartsWith("MyClass"))
                    {
                        Console.WriteLine($"{variable.Key} ({variable.Value.Type}):");
                        foreach (var field in (Dictionary<string, Variable>)variable.Value.Value)
                        {
                            Console.WriteLine($"  {field.Key} ({field.Value.Type}) = {field.Value.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{variable.Key} ({variable.Value.Type}) = {variable.Value.Value}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception={ex.Message}\n{ex.StackTrace}");
            }
        }


    }
}
