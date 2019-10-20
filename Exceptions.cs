using System;

namespace LogicSim
{

    public class InterpreterException : Exception
    {
        public InterpreterException(int lineNo, string message) : base($"Interpreter exception (line {lineNo}): {message}")
        {
            
        }
        
        public InterpreterException(string message) : base($"Interpreter exception: {message}")
        {
            
        }
    }
    
    public class InputVariableException : InterpreterException
    {
        public InputVariableException(int lineNo, string varName) : base(lineNo, $"Input variable [{varName}] has already been defined.")
        {
            
        }
    }
    
    public class OutputVariableException : InterpreterException
    {
        public OutputVariableException(int lineNo, string varName) : base(lineNo, $"Output variable [{varName}] has already been defined.")
        {
            
        }
    }
    
    public class MissingINPUTSException : InterpreterException
    {
        public MissingINPUTSException() : base($"File is missing INPUTS line")
        {
            
        }
    }
    
    public class MissingOUTPUTException : InterpreterException
    {
        public MissingOUTPUTException() : base($"File is missing OUTPUT line")
        {
            
        }
    }
}