using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace LogicSim
{
    public class Computer
    {
        //public static string initialCommand;
        public static int ComputeCircuit(string currentCommand)
        {
            
            
            // base case 1
            if (currentCommand == "0" || currentCommand == "1")
            {
                return Convert.ToInt16(currentCommand);
            }
            // base case 2
            else if (Interpreter.CountCommas(currentCommand) == 1)
            {
                List<string> commandArgs = Interpreter.GetCommandArgs(currentCommand);
                
                if (Interpreter.DetermineOuterCommand(currentCommand) == "AND")
                {
                    return Commands.AND(Convert.ToInt16(commandArgs[0]), Convert.ToInt16(commandArgs[1]));
                }
                else if (Interpreter.DetermineOuterCommand(currentCommand) == "OR")
                {
                    return Commands.OR(Convert.ToInt16(commandArgs[0]), Convert.ToInt16(commandArgs[1]));
                }
            }
            else if (Interpreter.CountCommas(currentCommand) > 1)
            { 
                List<string> commandArgs = Interpreter.GetCommandArgs(currentCommand);
                    if (Interpreter.DetermineOuterCommand(currentCommand) == "AND")
                    {
                        return Commands.AND(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                    }
                    if (Interpreter.DetermineOuterCommand(currentCommand) == "OR")
                    {
                        return Commands.OR(ComputeCircuit(commandArgs[0]), ComputeCircuit(commandArgs[1]));
                    }
                    
                

            }

            return 9999;
        }
    }
}