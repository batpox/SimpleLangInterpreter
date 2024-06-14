using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLangInterpreter
{
    /// <summary>
    /// Holds the context for the interpreter, including
    /// filepath and options.
    /// </summary>
    internal class SimpleLangContext
    {
        internal string? ScriptPath { get; set; }
        internal string? LogPath {  get; set; }  
        internal string? DebugPath {  get; set; }    

        internal SimpleLangContext()
        {
            ScriptPath = null;
            LogPath = null;
            DebugPath = null;
        }
    }
}
