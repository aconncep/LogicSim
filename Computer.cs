using System;
using System.Collections.Generic;

namespace LogicSim
{
    public class Computer
    {
        /// <summary>
        /// Recursive function for computing a command string composed of multiple commands
        /// Computes from the inside out and ultimately returns 0/1
        /// </summary>
        /// <returns>Computation (0/1) of current command</returns>
        public static int ComputeCircuit(string currentCommand, int currentLine)
        {
            // (base case) we're dealing with a hard-coded value
            if (currentCommand == "0" || currentCommand == "1")
            {
                return Convert.ToInt16(currentCommand);
            }

            // (base case) if this command is a variable, return that variables value
            Variable inputV = Interpreter.GetInputVariableWithName(currentCommand);
            Variable localV = Interpreter.GetLocalVariableWithName(currentCommand);
            if (inputV != null) { return inputV.Value ; }
            if (localV != null) { return localV.Value ; }


            // get a list of the arguments to this command
            List<string> commandArgs = Interpreter.GetCommandArgs(currentCommand);
            

            // this is a NOT command
            if (commandArgs.Count == 1)
            {
                return Commands.NOT(ComputeCircuit(commandArgs[0], currentLine));
            }
            
            // this is any other command (all of which requires two args)
            string outerCommand = StringUtilities.DetermineOuterCommand(currentCommand);
            switch (outerCommand)
            {
                case "AND":
                    return Commands.AND(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                case "OR":
                    return Commands.OR(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                case "NAND":
                    return Commands.NAND(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                case "NOR":
                    return Commands.NOR(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                case "XOR":
                    return Commands.XOR(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                case "XNOR":
                    return Commands.XNOR(ComputeCircuit(commandArgs[0], currentLine), ComputeCircuit(commandArgs[1], currentLine));
                default:
                    throw new InterpreterException(currentLine, $"Invalid command [{outerCommand}]");
            }
            
            
        }
    }
}