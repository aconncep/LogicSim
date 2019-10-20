using System;

namespace LogicSim
{

    public class Exception : System.Exception
    {
        public Exception(int lineNo, string message) : base($"Interpreter exception [line {lineNo}]: {message}")
        {
            
        }
    }
    
    public class InputVariableException : Exception
    {
        public InputVariableException(int lineNo, string varName) : base(lineNo, $"Variable {varName} has already been defined.")
        {
            
        }
    }
}