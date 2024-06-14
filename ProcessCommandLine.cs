using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLangInterpreter
{
    internal static class ProcessCommandLine
    {


        /// <summary>
        /// Parse the command line and return a context.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="langFile"></param>
        /// <param name="logPath"></param>
        /// <param name="debugPath"></param>
        /// <returns></returns>
        internal static bool Parse(string[] args, out SimpleLangContext context)
        {
            context = new SimpleLangContext();

            var options = new Dictionary<string, Action<SimpleLangContext,string>>()
        {
            { "/script", ProcessScriptPath },
            { "/help", ShowHelp },
            { "/debug", EnableDebug }
        };

            var arguments = new Dictionary<string, string?>();
            foreach (var arg in args)
            {
                if (arg.StartsWith('/'))
                {
                    string[] parts = arg.Substring(1).Split(':', 2);
                    string key = parts[0];
                    string? value = parts.Length > 1 ? parts[1] : null;
                    arguments[key] = value;
                }
            }
            // Get the path to the user's Documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return true;
        }


        /// <summary>
        /// Process the source argument.
        /// Use this to construct the log and debug files full paths.
        /// If debug is specified, set the console output to the debug file.
        /// </summary>
        /// <param name="scriptPath"></param>
        static void ProcessScriptPath(SimpleLangContext context, string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                Logging.LogIt($"No Source path given.");
                return;
            }

            if (!string.IsNullOrEmpty(scriptPath))
            {
                string? folder = Path.GetDirectoryName(scriptPath);
                if (folder == null)
                {
                    Logging.LogIt($"No such folder: {folder}");
                    return;
                }

                string? filename = Path.GetFileNameWithoutExtension(scriptPath);
                string? programPath = scriptPath;
                if (!File.Exists(programPath))
                {
                    Logging.LogIt($"Source file {programPath} cannot be found.");
                    return;
                }

                string logPath = Path.Combine(folder, filename, ".log");

                Logging.LogIt($"The source code path is: {scriptPath}. The log file is: {logPath}");

                // Define the debug output file path
                string debugPath = Path.Combine(folder, filename, ".debug");

                using (var writer = new StreamWriter(debugPath))
                {
                    Console.SetOut(writer);
                    Console.WriteLine("Simple Language runner.");

                    SimpleLangInterpreter.Run(programPath, logPath, debugPath);
                }
            }
        }
        static void ShowHelp(SimpleLangContext context, string arg)
        {
            Console.WriteLine("Usage: command /option:value /another-option:value");
            Console.WriteLine("Options:");
            Console.WriteLine("  /script:<filename>   Specify the script file to execute.");
            Console.WriteLine("  /debug:<true|false>  Enable or disable debug mode.");
            Console.WriteLine("  /help                Show this help message.");
        }



        static void EnableDebug(SimpleLangContext context, string arg)
        {
            Console.WriteLine("Debug mode enabled.");
        }
    }
}
