using System;
using System.Collections.Generic;

namespace LogicSim
{
    public static class RunModes
    {
        private static CircuitGroup _circuitGroup = new CircuitGroup();


        /// <summary>
        /// This function is called right when the program starts and a file is found.
        /// It populates the mainCircuit with circuit data to be used throughout the rest
        /// of the program's lifespan.
        /// </summary>
        /// <param name="fileLines"></param>
        public static void GenerateCircuit(string[] fileLines)
        {
            Interpreter.CheckForMissingLines(fileLines);
            Combination currentInputs = Interpreter.GetInputVariables(fileLines, out var numInputs);
            _circuitGroup.mainCircuit.NumberInputs = numInputs;

            for (int i = 0; i < Math.Pow(2, numInputs); i++)
            {
                currentInputs = Interpreter.GetInputVariables(fileLines, out var numberInpurts);
                string binaryStr = Convert.ToString(i, 2).PadLeft(numInputs, '0');

                int varIdx = 0;
                foreach (char c in binaryStr)
                {
                    currentInputs.combination[varIdx].Value = (int) Char.GetNumericValue(c);
                    varIdx++;
                }

                Combination currentOutputs = Interpreter.Interpret(currentInputs.combination, fileLines);

                _circuitGroup.mainCircuit.AddCombination(currentInputs, currentOutputs);
            }
        }


        public static void RunAuto()
        {
            Console.WriteLine("Simulating circuit...\n");
            _circuitGroup.mainCircuit.Print();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        public static void RunManual()
        {
            bool cont = true;
            while (cont)
            {
                Combination inputCombo = new Combination();
                for (int i = 0; i < _circuitGroup.mainCircuit.NumberInputs; i++)
                {
                    Variable currentInput = _circuitGroup.mainCircuit.GetInputVariables().combination[i];
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
                    inputCombo.SetVariable(currentInput.Name, result);
                }

                if (cont == true)
                {
                    Console.WriteLine(_circuitGroup.mainCircuit.GetOutputForInput(inputCombo));
                    Console.WriteLine();
                }
            }

            
        }

        public static int RunManualOnce(string variableName)
        {
            Console.Write("Enter value for input [" + variableName + "]: ");
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