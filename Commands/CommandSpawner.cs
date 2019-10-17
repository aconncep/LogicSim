using System.Net;
using System.Resources;

namespace LogicSim.Commands
{
    /// <summary>
    /// Takes the string representation of a command and creates a new command object of that command type
    /// (for example, this can turn the string "NOT" into a new NOT object)
    /// NOTE: This returns a blank command. Its arguments need to be set manually
    /// </summary>
    public static class CommandSpawner
    {
        public static ICommand GetCommandObjectFromString(string commandString, int lineNumber)
        {
            switch (commandString)
            {
                case "NOT":
                    return new NOT();
                
                case "AND":
                    return new AND();
                
                case "OR":
                    return new OR();
                
                case "NOR":
                    return new NOR();
                
                case "NAND":
                    return new NAND();

                case "XOR":
                    return new XOR();

                case "XNOR":
                    return new XNOR();
                
                default: // if the users desired command is not valid
                    throw new CompilationException("[" + commandString + "] is an invalid command", lineNumber);
            }
        }

    }
}