using System;
using System.Collections.Generic;

namespace LogicSim
{
    public static class Interpreter
    {
        private static int currentLine = 0;
        private static List<string> inputVariables = new List<string>();
        private static List<string> outputVariables = new List<string>();


        public static void Interpret(string[] fileLines)
        {
            
            CheckForMissingLines(fileLines);
            
            foreach (string line in fileLines)
            {
                currentLine++;
                
                string evalLine = line.Trim();

                if (evalLine.Length == 0)
                {
                    continue;
                }                
                
                if (LineStartsWithPound(evalLine))
                {
                    continue;
                }

                evalLine = RemoveInlineComments(evalLine);

                if (evalLine.StartsWith("INPUTS ", StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    string variablesStr = evalLine.Substring(7, evalLine.Length - 7);
                    
                    inputVariables = SplitInputsStringUp(variablesStr);
                    
                }
                
                if (evalLine.StartsWith("OUTPUT ", StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    string variablesStr = evalLine.Substring(7, evalLine.Length - 7);
                    
                    outputVariables = SplitOutputsStringUp(variablesStr);
                    
                }

                if (evalLine.Contains('='))
                {
                    evalLine = TrimVariableAssignment(evalLine);

                    int returnValue = Computer.ComputeCircuit(evalLine);
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
        
        private static bool LineStartsWithPound(string line)
        {
            return line[0].ToString() == "#";
        }

        private static string RemoveInlineComments(string line)
        {
            if (line.Contains('#'))
            {
                line = line.Substring(0, line.IndexOf('#'));
                if (line.EndsWith(' '))
                {
                    line = line.Substring(0, line.Length - 1);
                    return line;
                }
                return line;
            }
            return line;
        }
        

        private static string TrimVariableAssignment(string line)
        {
            int equalsIndex = line.IndexOf('=');
            return line.Substring(equalsIndex+1, (line.Length-1)-equalsIndex).Trim();
        }

        private static List<string> SplitInputsStringUp(string inputString)
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
                            inputVariables.Add(thisVar);
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
                        inputVariables.Add(thisVar);
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
        private static List<string> SplitOutputsStringUp(string outputString)
        {
            string thisVar = "";

            int idx = 0;
            
            foreach (char c in outputString)
            {
                if (c != 44) // if the character is not a comma
                {
                    thisVar += c;
                    if (idx == outputString.Length - 1)
                    {
                        if (!OutputAlreadyInList(thisVar))
                        {
                            outputVariables.Add(thisVar);
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
                        outputVariables.Add(thisVar);
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
            foreach (string inputVar in inputVariables)
            {
                if (inputVar == variable)
                {
                    exists = true;
                }
            }

            return exists;
        }
        
        private static bool OutputAlreadyInList(string variable)
        {
            bool exists = false;
            foreach (string outputVar in outputVariables)
            {
                if (outputVar == variable)
                {
                    exists = true;
                }
            }

            return exists;
        }

        public static string DetermineOuterCommand(string line)
        {
            int openParenthesisIndex = line.IndexOf('(');
            return line.Substring(0, openParenthesisIndex);
        }

        /*
        public static int CountCommas(string line)
        {
            int count = 0;
            foreach (char c in line)
            {
                if (c == ',')
                {
                    count++;
                }
            }

            return count;
        }
        */


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

            if (DetermineOuterCommand(line) == "NOT")
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
            else
            {
                commandArgs.Add(innerCommandString.Substring(0,FindThisCommandsComma(innerCommandString)));
                commandArgs.Add(innerCommandString.Substring(FindThisCommandsComma(innerCommandString)+1,innerCommandString.Length - commandArgs[0].Length-1));
            }
            
            return commandArgs;
        }

        
        /*
        private static int DetermineNumParenthesisGroups(string line)
        {
            // should probably make sure equal number of opening and closing parenthesis 
            
            int count = 0;
            foreach (char c in line)
            {
                if (c == '(')
                {
                    count++;
                }
            }

            return count;
        }
        */

        private static int FindThisCommandsComma(string line)
        {
            
            int firstParenthesisIdx = line.IndexOf('(') + 1;
            int firstCommaIdx = line.IndexOf(',');
            string trimmedLine = line.Substring(line.IndexOf('(')+1);
            int parenthesisCount = 1;
            int idx = 0;
            
            foreach (char c in trimmedLine)
            {
                idx++;
                
                if (c == '(')
                {
                    parenthesisCount++;
                }

                if (c == ')')
                {
                    parenthesisCount--;
                }

                if (parenthesisCount == 0)
                {
                    break;
                }
            }

            if (firstCommaIdx < firstParenthesisIdx )
            {
                return firstCommaIdx;
            }
            else
            {
                return firstParenthesisIdx + idx;
            }
        }
    }
}