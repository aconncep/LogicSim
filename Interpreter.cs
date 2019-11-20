using System;
using System.Collections.Generic;

namespace LogicSim
{
    public static class Interpreter
    {
        private static int currentLine;
        private static List<Variable> inputVariables = new List<Variable>();
        private static List<Variable> localVariables = new List<Variable>();
        private static List<Variable> outputVariables = new List<Variable>();
        
        public static void Interpret(string[] fileLines, bool auto, bool verbose)
        {
            CheckForMissingLines(fileLines);

            DetectInputVariables(fileLines);
            DetectOutputVariables(fileLines);


            bool printed = false;
            if (auto)
            {
                // each pass through the whole file with new inputs
                for (int i = 0; i < Math.Pow(2, inputVariables.Count); i++)
                {
                    string binaryStr = Convert.ToString(i, 2).PadLeft(inputVariables.Count, '0');
                    

                    SetInputVariableValues(binaryStr);
                    
                    if (verbose)
                    {
                        Console.WriteLine("Current input variables: ");
                        foreach (Variable inputV in inputVariables)
                        {
                            Console.Write(inputV.Name + ": " + inputV.Value + "  ");
                        }
                        Console.WriteLine();
                    }
                    
                    
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
                            string variableName = evalLine.Substring(0, evalLine.IndexOf('=')-1);
                    
                            evalLine = StringUtilities.TrimVariableAssignment(evalLine);

                            localVariables.Add(new Variable(variableName, Computer.ComputeCircuit(evalLine, currentLine)));
                        }
                    }

                    if (!printed)
                    {
                        if (!verbose)
                        {
                            foreach (Variable inputV in inputVariables)
                            {
                                Console.Write(inputV.Name + " ");
                            }
                            Console.Write("| ");
                            foreach (Variable outputV in outputVariables)
                            {
                                if (localVariables.Contains(outputV))
                                {
                                    Console.Write(outputV.Name + " ");
                                }
                            }
                            Console.WriteLine();
                        }

                        printed = true;
                    }
                    
                    // running in auto verbose
                    if (verbose)
                    {
                        foreach (Variable localV in localVariables)
                        {
                            if (outputVariables.Contains(localV))
                            {
                                Console.WriteLine("Output variable " + localV.Name + " with value " + localV.Value);
                            }

                            
                        } 
                        Console.WriteLine();
                    }
                    // running in auto simple
                    else
                    {
                        foreach (Variable inputV in inputVariables)
                        {
                            Console.Write(inputV.Value + " ");
                        }
                        Console.Write("| ");
                        foreach (Variable localV in localVariables)
                        {
                            if (outputVariables.Contains(localV))
                            {
                                Console.Write(localV.Value + " ");
                            }

                        }
                        Console.WriteLine();
                    }

                    localVariables.Clear(); // clear current localVarables list before running again (with new inputs)
                }
                
                
            }
            
            // running in manual
            else
            {
                while (true)
                {
                    string userBitString = "";
                    foreach (Variable inputV in inputVariables)
                    {
                        while (true)
                        {
                            Console.Write("Enter value for variable " + inputV.Name + " [or x to quit]:  ");
                            char userIn = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            if (userIn == 'x' || userIn == 'X')
                            {
                                Environment.Exit(0);
                            }
                            if (userIn != '0' && userIn != '1')
                            {
                                Console.WriteLine("Invalid input");
                                continue;
                            }
                            userBitString += userIn;
                            break;
                        }
                    }
                SetInputVariableValues(userBitString);

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

                        localVariables.Add(new Variable(variableName, Computer.ComputeCircuit(evalLine, currentLine)));
                    }
                }
                
                foreach (Variable localV in localVariables)
                {
                    if (outputVariables.Contains(localV))
                    {
                        Console.WriteLine("Output variable " + localV.Name + " with value " + localV.Value);
                    }

                            
                } 
                Console.WriteLine();
                localVariables.Clear();
                }
                
            }
        }
        
        private static void CheckForMissingLines(string[] fileLines)
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
        private static void SetInputVariableValues(string binaryStr)
        {
            int varIdx = 0;
            foreach (char c in binaryStr)
            {
                inputVariables[varIdx].Value = (int)Char.GetNumericValue(c);
                varIdx++;
            }
            
        }
        public static Variable GetInputVariableWithName(string name)
        {
            foreach (Variable var in inputVariables)
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
        
        private static void DetectInputVariables(string[] fileLines)
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

                    inputVariables = BuildInputVariables(variablesStr);

                }
            }
        }
        
        private static void DetectOutputVariables(string[] fileLines)
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

                    outputVariables = BuildOutputVariables(variablesStr);

                }
            }
        }
        
        private static List<Variable> BuildInputVariables(string inputString)
        {
            string thisVar = "";

            int idx = 0;
            
            foreach (char c in inputString)
            {
                if (c != 44) // if the character is not a comma
                {
                    thisVar += c;
                    if (idx == inputString.Length - 1)
                    {
                        if (!InputAlreadyInList(thisVar))
                        {
                            inputVariables.Add(new Variable(thisVar, 0));
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
                    if (!InputAlreadyInList(thisVar))
                    {
                        inputVariables.Add(new Variable(thisVar, 0));
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
            
            foreach (char c in inputString)
            {
                if (c != 44) // if the character is not a comma
                {
                    thisVar += c;
                    if (idx == inputString.Length - 1)
                    {
                        if (!OutputAlreadyInList(thisVar))
                        {
                            outputVariables.Add(new Variable(thisVar, 0));
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
                    if (!OutputAlreadyInList(thisVar))
                    {
                        outputVariables.Add(new Variable(thisVar, 0));
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

        private static bool InputAlreadyInList(string variable)
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

        private static bool OutputAlreadyInList(string variable)
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
        public static List<string> GetCommandArgs(string line)
        {
            string innerCommandString = line.Substring(line.IndexOf('(')+1, line.LastIndexOf(')') - line.IndexOf('(')-1);
            
            List<string> commandArgs = new List<string>();

            if (StringUtilities.DetermineOuterCommand(line) == "NOT")
            {
                commandArgs.Add(innerCommandString);
                return commandArgs;
            }
            
            if (innerCommandString == "0,0" || innerCommandString == "0,1" || innerCommandString == "1,0" || innerCommandString == "1,1")
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
                    if (c == "1")
                    {
                        commandArgs.Add("1");
                    }
                    else if (c == "0")
                    {
                        commandArgs.Add("0");
                    }
                    else if (GetInputVariableWithName(c) != null || GetLocalVariableWithName(c) != null)
                    {
                        Variable inputVar = GetInputVariableWithName(c);
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
                        throw new InterpreterException(currentLine, $"Undefined variable {c}");
                    }
                }
            }
            
            else
            {
                
                commandArgs.Add(innerCommandString.Substring(0,StringUtilities.FindThisCommandsComma(innerCommandString)));
                commandArgs.Add(innerCommandString.Substring(StringUtilities.FindThisCommandsComma(innerCommandString)+1,innerCommandString.Length - commandArgs[0].Length-1));
            }
            
            return commandArgs;
        }
    }
}