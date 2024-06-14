using Antlr4.Runtime.Misc;
using SimpleLangInterpreter;
using System;
using System.Collections.Generic;
using System.Linq;


internal class Program
{
    private static void Main(string[] args)
    {
        if (  ProcessCommandLine.Parse(args, out SimpleLangContext context))
        {

        }
    }
}
