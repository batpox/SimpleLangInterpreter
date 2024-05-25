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
            var input1 = @"int foo = 45;  string x = ""wassup""; 
                int bar = (77 - 5 + (5*5)) / 25; 
                int wiz = foo + 25; 
                "; // Example input

            var input2 = "int foo = (45 - 5 + (5*4))/20; int bar = foo+20;"; 

            string input9 = "int bar = 66; class MyClass1 { int x; string y; }; MyClass1 obj";

            var inputStream = new AntlrInputStream(input2);
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

        class SimpleLangVisitor : SimpleLangBaseVisitor<object>
        {
            public override object? VisitVarDecl(SimpleLangParser.VarDeclContext context)
            {
                var type = context.type().GetText();
                var id = context.ID().GetText();
                var expr = context.expr() != null ? context.expr().GetText() : "null";
                Console.WriteLine($"{type} {id} = {expr}");
                return base.VisitVarDecl(context);
            }

            public override object? VisitExpr(SimpleLangParser.ExprContext context)
            {
                Console.WriteLine($"Expression: {context.GetText()}");
                return base.VisitExpr(context);
            }
        }
    }
}
