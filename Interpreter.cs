using System;
using System.Collections.Generic;
using System.Threading;

namespace LogicSim
{
    public static class Interpreter
    {
        private static int currentLine; 
        public static readonly List<Variable> localVariables = new List<Variable>();

        /// <summary>
        /// take in a list of input variables (with their values set) and return
        /// a list of local variables after simulating the circuit
        /// </summary>
        public static List<Variable> Interpret(string[] fileLines, List<Variable> inputVars)
        {
            localVariables.Clear();
            List<Variable> localVariablesLocal = new List<Variable>();
            
            foreach (string line in fileLines)
            {
                currentLine++;

                string evalLine = line.Trim();

                if (evalLine.Length == 0 || line[0].ToString() == "#")
                {
                    continue;
                }

                evalLine = StringUtilities.RemoveInlineComments(evalLine);

                if (evalLine.Contains('='))
                {
                    string variableName = evalLine.Substring(0, evalLine.IndexOf('=') - 1);

                    evalLine = StringUtilities.TrimVariableAssignment(evalLine);

                    localVariablesLocal.Add(new Variable(variableName, Computer.ComputeCircuit(evalLine, currentLine, inputVars), VariableType.LOCAL));
                    localVariables.Add(new Variable(variableName, Computer.ComputeCircuit(evalLine, currentLine, inputVars), VariableType.LOCAL));
                }
            }
            return localVariablesLocal;
        }


        public static void CheckForMissingLines(string[] fileLines)
        {
            bool hasINPUTS = false;
            bool hasOUTPUT = false;

            foreach (string line in fileLines)
            {
                if (line.Trim().StartsWith("INPUTS ", StringComparison.CurrentCultureIgnoreCase))
                {
                    hasINPUTS = true;
                }

                if (line.Trim().StartsWith("OUTPUT ", StringComparison.CurrentCultureIgnoreCase))
                {
                    hasOUTPUT = true;
                }
            }

            if (hasINPUTS == false)
            {
                throw new MissingINPUTSException();
            }

            if (hasOUTPUT == false)
            {
                throw new MissingOUTPUTException();
            }
        }

        public static Variable GetInputVariableWithName(string name, List<Variable> inputVars)
        {
            foreach (Variable var in inputVars)
            {
                if (var.Name == name)
                {
                    return var;
                }
            }
            return null;
        }

        public static Variable GetLocalVariableWithName(string name)
        {
            foreach (Variable var in localVariables)
            {
                if (var.Name == name)
                {
                    return var;
                }
            }

            return null;
        }

        public static List<Variable> GetInputVariables(string[] fileLines)
        {
            foreach (string line in fileLines)
            {
                string evalLine = line.Trim();

                if (evalLine.Length == 0 || line[0].ToString() == "#")
                {
                    continue;
                }

                evalLine = StringUtilities.RemoveInlineComments(evalLine);
                if (evalLine.StartsWith("INPUTS ", StringComparison.CurrentCultureIgnoreCase))
                {
                    string variablesStr = evalLine.Substring(7, evalLine.Length - 7);
                    return new List<Variable>(BuildInputVariables(variablesStr));
                } 
            }

            return null;
        }

        public static List<Variable> GetOutputVariables(string[] fileLines)
        {
            foreach (string line in fileLines)
            {
                string evalLine = line.Trim();

                if (evalLine.Length == 0 || line[0].ToString() == "#")
                {
                    continue;
                }

                evalLine = StringUtilities.RemoveInlineComments(evalLine);
                if (evalLine.StartsWith("OUTPUT ", StringComparison.CurrentCultureIgnoreCase))
                {
                    string variablesStr = evalLine.Substring(7, evalLine.Length - 7);
                    return BuildOutputVariables(variablesStr);
                }
            }
            return null;
        }

        private static List<Variable> BuildInputVariables(string inputString)
        {
            string thisVar = "";

            int idx = 0;

            List<Variable> inputVariables = new List<Variable>();

            foreach (char c in inputString)
            {
                if (c != 44) // if the character is not a comma
                {
                    thisVar += c;
                    if (idx == inputString.Length - 1)
                    {
                        if (!InputAlreadyInList(thisVar, inputVariables))
                        {
                            inputVariables.Add(new Variable(thisVar, 0, VariableType.INPUT));
                            thisVar = "";
                        }
                        else
                        {
                            throw new InputVariableException(currentLine, thisVar);
                        }
                    }
                }
                else
                {
                    if (!InputAlreadyInList(thisVar, inputVariables))
                    {
                        inputVariables.Add(new Variable(thisVar, 0, VariableType.INPUT));
                        thisVar = "";
                    }
                    else
                    {
                        throw new InputVariableException(currentLine, thisVar);
                    }
                }

                idx++;
            }

            return inputVariables;
        }

        private static List<Variable> BuildOutputVariables(string inputString)
        {
            string thisVar = "";

            int idx = 0;

            List<Variable> outputVariables = new List<Variable>();

            foreach (char c in inputString)
            {
                if (c != 44) // if the character is not a comma
                {
                    thisVar += c;
                    if (idx == inputString.Length - 1)
                    {
                        if (!OutputAlreadyInList(thisVar, outputVariables))
                        {
                            outputVariables.Add(new Variable(thisVar, 0, VariableType.OUTPUT));
                            thisVar = "";
                        }
                        else
                        {
                            throw new OutputVariableException(currentLine, thisVar);
                        }
                    }
                }
                else
                {
                    if (!OutputAlreadyInList(thisVar, outputVariables))
                    {
                        outputVariables.Add(new Variable(thisVar, 0, VariableType.OUTPUT));
                        thisVar = "";
                    }
                    else
                    {
                        throw new OutputVariableException(currentLine, thisVar);
                    }
                }

                idx++;
            }
            return outputVariables;
        }

        private static bool InputAlreadyInList(string variable, List<Variable> inputVariables)
        {
            bool exists = false;
            foreach (Variable inputVar in inputVariables)
            {
                if (inputVar.Name == variable)
                {
                    exists = true;
                }
            }

            return exists;
        }

        private static bool OutputAlreadyInList(string variable, List<Variable> outputVariables)
        {
            bool exists = false;
            foreach (Variable outputVar in outputVariables)
            {
                if (outputVar.Name == variable)
                {
                    exists = true;
                }
            }

            return exists;
        }

        /// <summary>
        /// Given a command, returns a new list of strings where each string is a command argument
        /// Ex. AND(OR(A,B),NAND(C,D)) returns OR(A,B) and NAND(C,D)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static List<string> GetCommandArgs(string line, List<Variable> inputVariables)
        {
            string innerCommandString = "";
            try
            {
                innerCommandString =
                    line.Substring(line.IndexOf('(') + 1, line.LastIndexOf(')') - line.IndexOf('(') - 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InterpreterException($"Undefined variable [{line}]");
            }
            

            List<string> commandArgs = new List<string>();

            if (StringUtilities.DetermineOuterCommand(line) == "NOT")
            {
                commandArgs.Add(innerCommandString);
                if (commandArgs[0].Contains(',') && !commandArgs[0].Contains('('))
                {
                    throw new InterpreterException(currentLine, "Too many arguments passed to NOT command");
                }

                return commandArgs;
            }

            if (innerCommandString == "0,0" || innerCommandString == "0,1" || innerCommandString == "1,0" ||
                innerCommandString == "1,1")
            {
                foreach (char c in innerCommandString)
                {
                    if (c == 48 || c == 49) // 0 or 1
                    {
                        commandArgs.Add(c.ToString());
                    }
                }
            }


            if (!innerCommandString.Contains('('))
            {
                string[] innerCommandSplit = innerCommandString.Split(',');
                foreach (string c in innerCommandSplit)
                {
                    if (c == String.Empty)
                    {
                        throw new InterpreterException(currentLine, $"Missing argument for {line}");
                    }

                    switch (c)
                    {
                        case "1":
                            commandArgs.Add("1");
                            break;
                        case "0":
                            commandArgs.Add("0");
                            break;
                        default:
                        {
                            if (GetInputVariableWithName(c, inputVariables) != null ||
                                GetLocalVariableWithName(c) != null)
                            {
                                Variable inputVar = GetInputVariableWithName(c, inputVariables);
                                Variable localVar = GetLocalVariableWithName(c);
                                if (inputVar != null)
                                {
                                    commandArgs.Add(inputVar.Name);
                                }

                                if (localVar != null)
                                {
                                    commandArgs.Add(localVar.Name);
                                }
                            }
                            else
                            {
                                throw new InterpreterException(currentLine, $"Undefined variable [{c}]");
                            }

                            break;
                        }
                    }
                }
            }

            else
            {
                commandArgs.Add(innerCommandString.Substring(0,
                    StringUtilities.FindThisCommandsComma(innerCommandString)));
                commandArgs.Add(innerCommandString.Substring(
                    StringUtilities.FindThisCommandsComma(innerCommandString) + 1,
                    innerCommandString.Length - commandArgs[0].Length - 1));
            }

            return commandArgs;
        }
    }
}