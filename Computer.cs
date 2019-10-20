using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

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
            // we're dealing with a hard-coded value
            if (currentCommand == "0" || currentCommand == "1")
            {
                return Convert.ToInt16(currentCommand);
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
                    break;
                case "OR":
                    return Commands.OR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                    break;
                default:
                    return 2;
            }
            
            
        }
    }
}