internal class Program
{
    private static void Main(string[] args)
    {
        // Get the path to the user's Documents folder
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Define the output file path
        string outputFilePath = Path.Combine(documentsPath, "interpreterOutput.txt");

        using (var writer = new StreamWriter(outputFilePath))
        {
            Console.SetOut(writer);
            Console.WriteLine("Simple Language runner.");

            SimpleLangInterpreter.SimpleLangInterpreter.Run(args);
        }
    }
}