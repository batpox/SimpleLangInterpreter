internal class Program
{
    private static void Main(string[] args)
    {
        using (var writer = new StreamWriter(@"c:\temp\interpreterOutput.txt"))
        {
            Console.SetOut(writer);
            Console.WriteLine("Simple Language runner.");

            SimpleLangInterpreter.SimpleLangInterpreter.Run(args);
        }
    }
}