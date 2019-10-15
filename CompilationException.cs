using System;

namespace LogicSim
{
    public class CompilationException : Exception
    {
        public CompilationException(string message, int lineNumber) : base("Compilation Exception (Line " + lineNumber +  "): " + message)
        {
            
        }
        
        public CompilationException(string message) : base("Compilation Exception: " + message)
        {
            
        }
    }
}