using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicSim
{
    enum SpecialInputs
    {
        INVALID,
        EXIT
    }

    /// <summary>
    /// Methods for all of the main functionality the program is capable of
    /// </summary>
    public static class RunModes
    {
        /// <summary>
        /// This function is called right when the program starts and a file is found.
        /// It populates the mainCircuit with circuit data to be used throughout the rest
        /// of the program's lifespan.
        /// </summary>
        /// <param name="fileLines">string array containing the file's lines</param>
        public static void GenerateCircuit(string[] fileLines)
        {
            Interpreter.CheckForMissingLines(fileLines);
            List<Variable> currentInputs = Interpreter.GetInputVariables(fileLines);
            List<Variable> currentOutputs = Interpreter.GetOutputVariables(fileLines);
            int numInputs = currentInputs.Count;

            int currentComboNumber = 0;
            
            // cycle through binary
            for (int i = 0; i < Math.Pow(2, numInputs); i++)
            {
                currentInputs = Interpreter.GetInputVariables(fileLines);
                string binaryStr = Convert.ToString(i, 2).PadLeft(numInputs, '0');

                int varIdx = 0;
                foreach (char c in binaryStr)
                {
                    currentInputs[varIdx].Value = (int) Char.GetNumericValue(c);
                    varIdx++;
                }

                // take the currently set inputs and return a list of local variables after interpreting the file with those inputs
                List<Variable> currentLocals = Interpreter.Interpret(fileLines, currentInputs);

                
                foreach (Variable var in currentLocals)
                {
                    if (currentOutputs.Contains(var))
                    {
                        currentOutputs[currentOutputs.IndexOf(var)].type = VariableType.USED_OUTPUT;
                        var.type = VariableType.OUTPUT;
                    }
                }


                // create a new combo out of the current inputs and locals
                Combination completeCombo = new Combination(currentInputs, currentLocals);

                // add the new combo to the circuit
                CircuitGroup.mainCircuit.AddCombination(currentComboNumber, completeCombo);

                currentComboNumber++;
            }

            foreach (Variable outputV in currentOutputs)
            {
                if (outputV.type != VariableType.USED_OUTPUT)
                {
                    Console.WriteLine($"Warning: Undeclared variable [{outputV.Name}] is marked as output. It will be ignored.");
                }
            }

        }

        /// <summary>
        /// Auto mode, which prints the entire circuit immediately
        /// </summary>
        public static void RunAuto()
        {
            while (true)
            {
                Console.Write("Enter simulation delay (0 for instantaneous): ");
                try
                {
                    int delay = int.Parse(Console.ReadLine());
                    if (delay < 0)
                    {
                        Console.WriteLine("Invalid input. Try again.\n");
                        continue;
                    }

                    if (delay == 0)
                    {
                        RunAutoNoDelay();
                    }
                    else
                    {
                        Console.WriteLine("Simulating circuit...");
                        CircuitGroup.mainCircuit.PrintWithDelay(delay);
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...\n");
                        Console.ReadKey();
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }
        
        /// <summary>
        /// Print the whole circuit with no delay in between prints
        /// </summary>
        public static void RunAutoNoDelay()
        {
            Console.WriteLine("Simulating circuit...");
            Console.WriteLine(CircuitGroup.mainCircuit);
            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// Manual mode, which asks for a set of inputs then prints the appropriate outputs
        /// </summary>
        public static void RunManual()
        {
            bool cont = true;
            while (cont)
            {
                List<Variable> newInputs = new List<Variable>();
                for (int i = 0; i < CircuitGroup.mainCircuit.NumInputs; i++)
                {
                    Variable currentInput = CircuitGroup.mainCircuit.GetInputVariables()[i];
                    var result = RunManualOnce(currentInput.Name);

                    if (result.Equals(SpecialInputs.EXIT))
                    {
                        cont = false;
                        break;
                    }

                    if (result.Equals(SpecialInputs.INVALID))
                    {
                        while (true)
                        {
                            result = RunManualOnce(currentInput.Name);
                            if (result.Equals(SpecialInputs.EXIT))
                            {
                                cont = false;
                                break; 
                            }
                            if (result.Equals(SpecialInputs.INVALID))
                            {
                                continue;
                            }
                            break;
                        }
                        
                    }

                    if (cont)
                    {
                        newInputs.Add(new Variable(currentInput.Name, result, VariableType.INPUT));
                    }
                    else
                    {
                        break;
                    }
                    
                }

                if (cont)
                {
                    CircuitGroup.mainCircuit.PrintIndividualComboOutput(newInputs);
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Ask the user to set a specific input variable's value
        /// </summary>
        /// <param name="variableName">The name of the input variable being set</param>
        /// <returns>The value entered by the user, or EXIT/INVALID in special cases</returns>
        public static dynamic RunManualOnce(string variableName)
        {
            Console.Write("Enter value for input [" + variableName + "] (or x to quit): ");
            string userIn = Console.ReadLine();

            if (userIn == "x" || userIn == "X")
            {
                Console.WriteLine("Returning to main menu...");
                Console.WriteLine();
                return SpecialInputs.EXIT;
            }

            int userInInt;
            try
            {
                userInInt = int.Parse(userIn);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                return SpecialInputs.INVALID;
            }

            if (userInInt == 0 || userInInt == 1)
            {
                return userInInt;
            }

            Console.WriteLine("Invalid input. Try again.");
            return SpecialInputs.INVALID;
        }

        /// <summary>
        /// Steps through each input as the user presses SPACE
        /// </summary>
        public static void StepThroughInputs()
        {
            Console.WriteLine("Repeatedly press SPACE to simulate the next input combo [or x to quit]...\n");
            Console.WriteLine(CircuitGroup.mainCircuit.GetTitleLine());
            int idx = 0;
            while (true)
            {
                if (idx == Math.Pow(2, CircuitGroup.mainCircuit.NumInputs))
                {
                    Console.WriteLine("\nPress any key to continue...\n");
                    Console.ReadKey(true);
                    break;
                }
                ConsoleKeyInfo userIn = Console.ReadKey(true);
                if (userIn.Key.Equals(ConsoleKey.Spacebar))
                {
                    Console.WriteLine(CircuitGroup.mainCircuit.GetEntireLine(idx));
                    idx++;
                }
                else if (userIn.Key.Equals(ConsoleKey.X))
                {
                    Console.WriteLine("\n");
                    break;
                }
            }
        }

        /// <summary>
        /// Show all combinations that create all 0s or 1s
        /// </summary>
        /// <param name="outputToShow">0 or 1</param>
        public static void ShowOnlyOutput(int outputToShow)
        {
            if (outputToShow == 0 || outputToShow == 1)
            {
                List<Variable> desiredOutputs = new List<Variable>();
                foreach (Variable var in CircuitGroup.mainCircuit.GetOutputs())
                {
                    Variable newVar = new Variable(var.Name, outputToShow, VariableType.OUTPUT);
                    desiredOutputs.Add(newVar);
                }

                CircuitGroup.mainCircuit.PrintCombosWithOutput(desiredOutputs);
            }

            Console.WriteLine("Press any key to continue...\n");
            Console.ReadKey();
        }


        public static void HoldInputConstant()
        {
            while (true)
            {
                List<Variable> currentInputs = CircuitGroup.mainCircuit.GetInputVariables();
                string[] currentInputsSelections = new string[currentInputs.Count];
                int i = 0;
                foreach (Variable var in currentInputs)
                {
                    currentInputsSelections[i] = var.Name;
                    i++;
                }
                var userSelection = MenuSelection.PromptNoQuestion(currentInputsSelections, false);

                if (userSelection.Equals(Selection.QUIT))
                {
                    break;
                }

                if (!(userSelection >= 0) || !(userSelection < currentInputs.Count))
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                else
                {
                    int bitChoice = -1;
                    while (true)
                    {
                        Console.Write($"Hold input [{currentInputs[userSelection].Name}] at 0 or 1?: ");
                        ConsoleKeyInfo userChoice = Console.ReadKey();
                        // check if is digit first? Hitting exception below

                        if (int.Parse(userChoice.KeyChar.ToString()) != 0 && int.Parse(userChoice.KeyChar.ToString()) != 1)
                        {
                            Console.WriteLine("Invalid input. Try again");
                        }
                        else
                        {
                            bitChoice = int.Parse(userChoice.KeyChar.ToString());
                            break;
                        }
                    }
                    
                    Console.WriteLine($"Valid input! You picked {currentInputs[userSelection].Name} with bit {bitChoice}");
                    CircuitGroup.mainCircuit.PrintCombosWithHeldInput(currentInputs[userSelection], bitChoice);
                    break;
                }
            }
            
            
        }
    }
}