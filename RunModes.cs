using System;
using System.Collections.Generic;

namespace LogicSim
{
    public static class RunModes
    {
        /// <summary>
        /// This function is called right when the program starts and a file is found.
        /// It populates the mainCircuit with circuit data to be used throughout the rest
        /// of the program's lifespan.
        /// </summary>
        /// <param name="fileLines"></param>
        public static void GenerateCircuit(string[] fileLines)
        {
            Interpreter.CheckForMissingLines(fileLines);
            List<Variable> currentInputs = Interpreter.GetInputVariables(fileLines);
            int numInputs = currentInputs.Count;

            int currentComboNumber = 0;
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

                // create a new combo out of the current inputs and outputs
                Combination completeCombo = new Combination(currentInputs, currentLocals);

                // add the new combo to the circuit
                CircuitGroup.mainCircuit.AddCombination(currentComboNumber, completeCombo);

                currentComboNumber++;
            }
        }

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
                        Console.WriteLine("Simulating circuit...");
                        Console.WriteLine(CircuitGroup.mainCircuit);
                        Console.WriteLine("Press any key to continue...");
                        Console.WriteLine();
                        Console.ReadKey();
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

        public static void RunManual()
        {
            bool cont = true;
            while (cont)
            {
                List<Variable> newInputs = new List<Variable>();
                for (int i = 0; i < CircuitGroup.mainCircuit.NumInputs; i++)
                {
                    Variable currentInput = CircuitGroup.mainCircuit.GetInputVariables()[i];
                    int result = RunManualOnce(currentInput.Name);
                    if (result == -2)
                    {
                        cont = false;
                        break;
                    }
                    if (result == -1)
                    {
                        result = RunManualOnce(currentInput.Name);
                    }
                    newInputs.Add(new Variable(currentInput.Name, result, VariableType.INPUT));
                }

                if (cont == true)
                {
                    CircuitGroup.mainCircuit.PrintIndividualComboOutput(newInputs);
                    Console.WriteLine();
                }
            }

            
        }

        public static int RunManualOnce(string variableName)
        {
            Console.Write("Enter value for input [" + variableName + "] (or x to quit): ");
            string userIn = Console.ReadLine();

            if (userIn == "x" || userIn == "X")
            {
                Console.WriteLine("Returning to main menu...");
                Console.WriteLine();
                return -2;
            }
            
            int userInInt;
            try
            {
                userInInt = int.Parse(userIn);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Try again.");
                return -1;
            }

            if (userInInt == 0 || userInInt == 1)
            {
                return userInInt;
            }
            Console.WriteLine("Invalid input. Try again.");
            return -1;
        }
    }
    
}