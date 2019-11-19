using System;
using System.Collections.Generic;

namespace LogicSim
{
    public class Computer
    {
        /// <summary>
        /// Recursive function for computing a command string composed of multiple commands
        /// Computes from the inside out and returns the output
        /// </summary>
        /// <param name="currentCommand"></param>
        /// <returns></returns>
        public static int ComputeCircuit(string currentCommand)
        {
            // we're dealing with a hard-coded value (base case)
            if (currentCommand == "0" || currentCommand == "1")
            {
                return Convert.ToInt16(currentCommand);
            }

            Variable inputV = Interpreter.GetInputVariableWithName(currentCommand);
            Variable localV = Interpreter.GetLocalVariableWithName(currentCommand);
            if (inputV != null)
            {
                return inputV.value;
            }

            if (localV != null)
            {
                return localV.value;
            }
            
            
            List<string> commandArgs = Interpreter.GetCommandArgs(currentCommand);
            

            // this is a NOT command
            if (commandArgs.Count == 1)
            {
                return Commands.NOT(ComputeCircuit(commandArgs[0]));
            }
            
            // this is any other command (all of which requires two args)
            string outerCommand = Interpreter.DetermineOuterCommand(currentCommand);
            switch (outerCommand)
            {
                case "AND":
                    return Commands.AND(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                case "OR":
                    return Commands.OR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                case "NAND":
                    return Commands.NAND(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                case "NOR":
                    return Commands.NOR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                case "XOR":
                    return Commands.XOR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                case "XNOR":
                    return Commands.XNOR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                default:
                    return 2;
            }
            
            
        }
    }
}