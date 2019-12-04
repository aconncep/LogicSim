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
                    currentInputs.combination[varIdx].Value = (int)Char.GetNumericValue(c);
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
            Combination inputCombo = new Combination();
            for (int i = 0; i < _circuitGroup.mainCircuit.NumberInputs; i++)
            {
                Variable currentInput = _circuitGroup.mainCircuit.GetInputVariables().combination[i];
                Console.Write("Enter a value for input variable [" + currentInput.Name+"]: ");
                int userIn = int.Parse(Console.ReadLine());
                inputCombo.SetVariable(currentInput.Name, userIn);
            }
            Console.WriteLine(_circuitGroup.mainCircuit.GetOutputForInput(inputCombo));
        }
        
    }
}