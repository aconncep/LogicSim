using System;
using System.Collections.Generic;
using System.Text;

namespace LogicSim
{
    public class Computer
    {
        /// <summary>
        /// Recursive function for computing a command string composed of multiple commands
        /// Computes from the inside out and ultimately returns 0/1
        /// </summary>
        /// <returns>Computation (0/1) of current command</returns>
        public static int ComputeCircuit(string currentCommand, int currentLine, List<Variable> inputVars)
        {
            //CircuitGroup.mainCircuit.TracedComputation.Add($"Current command is {currentCommand}");

            // (base case) we're dealing with a hard-coded value
            if (currentCommand == "0" || currentCommand == "1")
            {
                int command = Convert.ToInt16(currentCommand);
                CircuitGroup.mainCircuit.TracedComputation.Add($"Command is a hard-coded {command}");
                return command;
            }

            // (base case) if this command is a variable, return that variable's value
            Variable inputV = Interpreter.GetInputVariableWithName(currentCommand, inputVars);
            Variable localV = Interpreter.GetLocalVariableWithName(currentCommand);
            if (inputV != null) 
            { 
                CircuitGroup.mainCircuit.TracedComputation.Add($"Replacing input variable [{inputV.Name}] with value {inputV.Value}");
                return inputV.Value; 
            }

            if (localV != null)
            {
                CircuitGroup.mainCircuit.TracedComputation.Add($"Replacing local variable [{localV.Name}] with value {localV.Value}");
                return localV.Value;
            }


            // get a list of the arguments to this command
            List<string> commandArgs = Interpreter.GetCommandArgs(currentCommand, inputVars);
            

            // this is a NOT command
            if (commandArgs.Count == 1)
            {
                int result = Commands.NOT(ComputeCircuit(commandArgs[0], currentLine, inputVars));
                CircuitGroup.mainCircuit.TracedComputation.Add($"Computing NOT({commandArgs[0]}) returned {result}");
                return result;

            }
            
            // this is any other command (all of which requires two args)
            string outerCommand = StringUtilities.DetermineOuterCommand(currentCommand);
            switch (outerCommand)
            {
                case "AND":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing AND({commandArgs[0]},{commandArgs[1]})");
                    int andResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int andResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalAndResult = Commands.AND(andResult1, andResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing AND({commandArgs[0]}={andResult1},{commandArgs[1]}={andResult2}) returned {finalAndResult}");
                    return finalAndResult;
                case "OR":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing OR({commandArgs[0]},{commandArgs[1]})");
                    int orResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int orResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalOrResult = Commands.OR(orResult1, orResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add(
                        $"Computing OR({commandArgs[0]}={orResult1},{commandArgs[1]}={orResult2}) returned {finalOrResult}");
                    return finalOrResult;
                case "NAND":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing NAND({commandArgs[0]},{commandArgs[1]})");
                    int nandResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int nandResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalNandResult = Commands.NAND(nandResult1, nandResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing NAND({commandArgs[0]}={nandResult1},{commandArgs[1]}={nandResult2}) returned {finalNandResult}");
                    return finalNandResult;
                case "NOR":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing NOR({commandArgs[0]},{commandArgs[1]})");
                    int norResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int norResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalNorResult = Commands.NOR(norResult1, norResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing NOR({commandArgs[0]}={norResult1},{commandArgs[1]}={norResult2}) returned {finalNorResult}");
                    return finalNorResult;
                case "XOR":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing XOR({commandArgs[0]},{commandArgs[1]})");
                    int xorResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int xorResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalXorResult = Commands.XOR(xorResult1, xorResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing XOR({commandArgs[0]}={xorResult1},{commandArgs[1]}={xorResult2}) returned {finalXorResult}");
                    return finalXorResult;
                case "XNOR":
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing XNOR({commandArgs[0]},{commandArgs[1]})");
                    int xnorResult1 = ComputeCircuit(commandArgs[0], currentLine, inputVars);
                    int xnorResult2 = ComputeCircuit(commandArgs[1], currentLine, inputVars);
                    int finalXnorResult = Commands.XNOR(xnorResult1, xnorResult2);
                    CircuitGroup.mainCircuit.TracedComputation.Add($"Computing XNOR({commandArgs[0]}={xnorResult1},{commandArgs[1]}={xnorResult2}) returned {finalXnorResult}");
                    return finalXnorResult;
                default:
                    throw new InterpreterException(currentLine, $"Invalid command [{outerCommand}]");
            }
        }
    }
}