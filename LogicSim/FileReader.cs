using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using LogicSim.Commands;

namespace LogicSim
{
    /// <summary>
    /// Processes a program file and generates the data needed for the Computer class to process the circuit
    /// This is the "interpreter"
    /// </summary>
    public class FileReader
    {
        private string[] lines;

        // Stores the local output variable (w1,w2,...) as the key, and the command (w/ associated input variables) as the value
        private Dictionary<Variable, ICommand> localVars = new Dictionary<Variable, ICommand>();
        
        // List of all the input variables declared in the INPUTS line
        private List<Variable> inputVariables = new List<Variable>();
        
        // List of all the output variables. This data stays in this class, and is used to set the shouldOutput field of certain variables to true
        private List<Variable> outputVariables = new List<Variable>();
        
        // List of all the local variable names that have been declared so far.
        // This is used to make sure the user does not try to use a local variable that has not been declared.
        private List<string> declaredLocalVars = new List<string>();

        private List<Variable> allArgVars = new List<Variable>();

        int currentLineNum = 0; // the current line number being processed, used for reporting errors to the user

        /// <summary>
        /// Set the path to file and populate the validCommands dictionary
        /// </summary>
        /// <param name="pathToFile"></param>
        public FileReader(string pathToFile)
        {
            lines = File.ReadAllLines(pathToFile);
        }

        /// <summary>
        /// Reads in each line of the input file and generates file data for use by the Computer class
        /// </summary>
        /// <returns>A FileData object with localVariable dictionary and inputVariables list</returns>
        /// <exception cref="CompilationException">If there is an issue with the input file</exception>
        public FileData GenerateFileData()
        {
            

            // we assume that the file does not have these two required lines. If we find them, the variable changes to true
            bool hasInputsLine = false;
            bool hasOutputLine = false;

            foreach (string line in lines)
            {
                currentLineNum++;

                string currentLine = line.Trim(); // trim white space off each line

                if (IsCommentLine(currentLine) || string.IsNullOrWhiteSpace(currentLine))
                {
                    continue;
                } // if the line is blank or starts with a #, ignore it

                currentLine =
                    RemoveCommentFromLine(
                        currentLine); // truncate the end of a line if it has in-line comments (so only the non-comment portion remains)

                // if this line is the INPUTS line, update the boolean variable and generate Variables for the inputVariables list
                if (LineStartsWithINPUTS(line))
                {
                    // if an inputs line has already been discovered, throw an exception. A file can only have one inputs line.
                    if (hasInputsLine == true)
                    {
                        throw new CompilationException("Multiple INPUT lines", currentLineNum);
                    }

                    hasInputsLine = true;
                    string[] inputVariableStr = GetInputVariables(currentLine); // get a string array of the input variable names

                    // for each variable, create a new Variable object and add it to the inputVariables list
                    foreach (string variable in inputVariableStr)
                    {
                        Variable var = new Variable(VarType.Input, variable, false);
                        inputVariables.Add(var);
                    }

                }
                // create a list of the variables that need to be outputted
                else if (LineStartsWithOUTPUT(line))
                {
                    hasOutputLine = true;
                    string[] outputVariableStr = GetOutputVariables(currentLine); // get a string array of the variable names
                    
                    // for each variable, create a new Variable object and add it to the outputVariables list
                    foreach (string variable in outputVariableStr)
                    {
                        Variable var = new Variable(VarType.Local, variable, true);
                        outputVariables.Add(var);
                    }
                }
                else // this line is not INPUTS, OUTPUTS, or a comment
                {
                    if (!LineContainsEquals(currentLine)) // if the line is some invalid giberrish
                    {
                        throw new CompilationException("Error processing line [" + currentLine + "]", currentLineNum);
                    }

                    if (!LineHasCompleteParenthesis(currentLine)) // if the line does not have one complete set of parenthesis ( )
                    {
                        throw new CompilationException("Missing complete parenthesis set", currentLineNum);
                    }

                    // get the string representing the variable name (like w1 or w2)
                    string variableName = GetVariableName(currentLine);

                    if (string.IsNullOrWhiteSpace(variableName)) // if the variable name is blank
                    {
                        throw new CompilationException("Variable name cannot be blank", currentLineNum);
                    }
                    
                    declaredLocalVars.Add(variableName);
                    
                    // get the string representation of the desired command, then creates a command object from it
                    string commandName = GetCommandName(currentLine);
                    ICommand desiredCommand = CommandSpawner.GetCommandObjectFromString(commandName, currentLineNum);
              

                    string[] arguments = GetArgumentsList(currentLine, desiredCommand); // get the arguments the user wants to pass to the command
                    Variable[] varArgs = new Variable[arguments.Length]; // create an empty Variable array with enough room to hold each desired argument
                    

                    // Create new Variable objects and store them in varArgs array
                    int idx = 0;
                    foreach (string arg in arguments)
                    {
                        varArgs[idx] = new Variable(VarType.Local, arg, false);
                        
                        // if the user is trying to use a variable that hasn't been declared (either one of the inputs or a local var that was created)
                        if (!declaredLocalVars.Contains(varArgs[idx].name) && !inputVariables.Contains(varArgs[idx]))
                        {
                            throw new CompilationException("Variable [" + varArgs[idx].name + "] does not exist.", currentLineNum);
                        }
                        
                        idx++;
                    }
                    
                    desiredCommand.SetVariableValues(varArgs); // set the variables that this command will use to process
                    
                    // create a new dictionary entry with a variable (like w1) and it's associated command
                    localVars.Add(new Variable(VarType.Local, variableName, false), desiredCommand);
                    
                }
   
            }
            
            // if the user is missing one of the required lines
            if (hasInputsLine == false)
            {
                throw new CompilationException("Missing INPUTS line");
            }

            if (hasOutputLine == false)
            {
                throw new CompilationException("Missing OUTPUT line. Each program must have at least one.");
            }
            
            // populate the outputVariables list
            SetOutputVariables();
            
            CheckFileForWarnings();
            
            return new FileData(localVars, inputVariables);
        }

        // is this a comment line?
        private bool IsCommentLine(string line)
        {
            return line.StartsWith("#", StringComparison.CurrentCulture);
        }

        // Remove the comment from a line with an in-line comment
        private string RemoveCommentFromLine(string line)
        {
            if (!line.Contains('#'))
            {
                return line;
            }

            int poundIndex = line.IndexOf('#');
            return line.Substring(0, poundIndex);
        }

        // Does this line contain a = ?
        private bool LineContainsEquals(string line)
        {
            return line.Contains('=');
        }

        // Does this line start with INPUTS?
        private bool LineStartsWithINPUTS(string line)
        {
            if (line.StartsWith("INPUTS", StringComparison.CurrentCulture))
            {
                return true;
            }
            if (line.StartsWith("INPUTS", StringComparison.CurrentCultureIgnoreCase))
            {
                string wrongInput = line.Split(' ').First();
                throw new CompilationException("Error processing [" + wrongInput + "]. Did you mean INPUTS?", currentLineNum);

            }
            return false;
        }

        // Does this line start with OUTPUT?
        private bool LineStartsWithOUTPUT(string line)
        {
            if (line.StartsWith("OUTPUT", StringComparison.CurrentCulture))
            {
                return true;
            }
            if (line.StartsWith("OUTPUT", StringComparison.CurrentCultureIgnoreCase))
            {
                string wrongInput = line.Split(' ').First();
                throw new CompilationException("Error processing [" + wrongInput + "]. Did you mean OUTPUT?", currentLineNum);

            }
            return false;
        }

        // Does this line contain a complete set of parenthesis ( ) ?
        private bool LineHasCompleteParenthesis(string line)
        {
            bool oneOpeningP = line.IndexOf('(') == line.LastIndexOf('(');
            bool oneClosingP = line.IndexOf(')') == line.LastIndexOf(')');
            if (oneOpeningP && oneClosingP)
            {
                return line.IndexOf('(') < line.IndexOf(')');
            }
            return false;
        }

        // Get the name of the variable the user is trying to declare
        private string GetVariableName(string line)
        {
            return line.Substring(0, line.IndexOf('=')).Trim();
        }

        // Get the name of the command the user is trying to use
        private string GetCommandName(string line)
        {
            return line.Substring(line.IndexOf('=')+1, line.IndexOf('(')-line.IndexOf('=')-1).Trim();

        }

        // Get the list of arguments the user is trying to pass to the command
        private string[] GetArgumentsList(string line, ICommand command)
        {
            string betweenParenthesis = line.Substring(line.IndexOf('(')+1, line.IndexOf(')')-line.IndexOf('(')-1).Trim();

            // if one of the arguments is blank
            foreach (string arg in betweenParenthesis.Split(','))
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    throw new CompilationException("Invalid syntax in [" + betweenParenthesis + "]", currentLineNum);
                }
            }
            
            // if the wrong number of arguments is supplied
            if (betweenParenthesis.Split(',').Length != command.ExpectedNumArguments())
            {
                throw new CompilationException("Invalid syntax in [" + betweenParenthesis + "]", currentLineNum);
            }
            
            return betweenParenthesis.Split(',');
        }

        // Get the list of input variables the user is creating
        private string[] GetInputVariables(string line)
        {
            string substr = line.Substring(6, line.Length-6).Trim();
            return substr.Split(',');

        }

        // Get the list of variables the user wants to use as outputs
        private string[] GetOutputVariables(string line)
        {
            string substr = line.Substring(6, line.Length-6).Trim();
            string[] attemptedOutputVars = substr.Split(',');

            // if any attempted output variables don't exist
            foreach (string var in attemptedOutputVars)
            {
                if (!declaredLocalVars.Contains(var) && !inputVariables.Contains(new Variable(VarType.Input, var, false)))
                {
                    throw new CompilationException("Cannot output unknown variable [" + var + "]", currentLineNum);
                }
            }

            return attemptedOutputVars;
        }

        // Populate the outputVariables list with the desired output variables
        private void SetOutputVariables()
        {
            foreach(Variable var in localVars.Keys)
            {
                if (outputVariables.Contains(var))
                {
                    var.shouldOutput = true;
                }
            }

            foreach (Variable var in inputVariables)
            {
                if (outputVariables.Contains(var))
                {
                    var.shouldOutput = true;
                }
            }
        }

        /// <summary>
        /// Checks a file for warnings such as
        /// 1. Variable is declared that is never used
        /// </summary>
        private void CheckFileForWarnings()
        {
            foreach (ICommand command in localVars.Values)
            {
                foreach (Variable var in command.GetVariables())
                {
                    allArgVars.Add(var);
                } 
            }
            
            foreach (Variable var in inputVariables)
            {
                if (!localVars.ContainsKey(var) && !allArgVars.Contains(var))
                {
                    Console.WriteLine("Warning: Input variable [" + var.name + "] is assigned but never used");
                }
            }

            foreach (Variable var in localVars.Keys)
            {
                if (!allArgVars.Contains(var) && !outputVariables.Contains(var))
                {
                    Console.WriteLine("Warning: Local variable [" + var.name + "] is assigned but never used");
                }
            }
        }
        



    }
}