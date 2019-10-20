using System;
using System.Collections.Generic;

namespace LogicSim
{
    public static class Interpreter
    { 
        public static void Interpret(string[] fileLines)
        {
            foreach (string line in fileLines)
            {
                string evalLine = line.Trim();

                if (evalLine.Length == 0)
                {
                    continue;
                }                
                
                if (LineStartsWithPound(evalLine))
                {
                    continue;
                }

                if (evalLine.Contains('='))
                {
                    evalLine = TrimVariableAssignment(evalLine);
                    Console.WriteLine("Line being evaluated: " + evalLine);
                    

                    //Computer.initialCommand = DetermineOuterCommand(evalLine);
                    Console.WriteLine(Computer.ComputeCircuit(evalLine));
                }
                
                
                

            }
        }
        
        private static bool LineStartsWithPound(string line)
        {
            return line[0].ToString() == "#";
        }

        private static string TrimVariableAssignment(string line)
        {
            int equalsIndex = line.IndexOf('=');
            return line.Substring(equalsIndex+1, (line.Length-1)-equalsIndex).Trim();
        }

        public static string DetermineOuterCommand(string line)
        {
            int openParenthesisIndex = line.IndexOf('(');
            return line.Substring(0, openParenthesisIndex);
        }

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

        /// <summary>
        /// Given a command, returns a new list of strings where each string is a command argument
        /// Ex. AND(OR(A,B),NAND(C,D)) returns OR(A,B) and NAND(C,D)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static List<string> GetCommandArgs(string line)
        {
            string innerCommandString = line.Substring(line.IndexOf('(')+1, line.LastIndexOf(')') - line.IndexOf('(')-1);
            Console.WriteLine("Current inner command string: " + innerCommandString);
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