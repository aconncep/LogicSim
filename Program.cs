using System;
using System.IO;
using System.Collections.Generic;

namespace LogicSim
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                FileReader reader = null;
                try
                {
                    reader = new FileReader(args[0]);  
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Error: File not found");
                    Environment.Exit(1);
                }

                FileData fileData = null;

                try
                {
                    fileData = reader.GenerateFileData();
                }
                catch (CompilationException e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                if (String.Equals(args[1], "auto", StringComparison.Ordinal))
                {
                    if (String.Equals(args[2], "verbose", StringComparison.Ordinal))
                    {
                        RunAuto(fileData, true);
                    }
                    else if (String.Equals(args[2], "simple", StringComparison.Ordinal))
                    {
                        RunAuto(fileData, false);
                    }
                    else
                    {
                        Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
                    }


                }
                else if (String.Equals(args[1], "manual", StringComparison.Ordinal))
                {
                    if (String.Equals(args[2], "verbose", StringComparison.Ordinal))
                    {
                        RunManual(fileData);
                    }
                    else if (String.Equals(args[2], "simple", StringComparison.Ordinal))
                    {
                        Console.WriteLine("Manual simple mode is not supported. Using manual verbose instead...");
                        RunManual(fileData);
                    }
                    else
                    {
                        Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
                    }
                }
                else
                {
                    Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
                }
            }
            else
            {
                Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
            }
        }

        static void RunManual(FileData fileData)
        {

            int currentVariableIdx = 0;
            
            while (currentVariableIdx < fileData.inputVariables.Count)
            {
                Console.Write("Enter value for input variable " + fileData.inputVariables[currentVariableIdx].name + ": ");
                string userInput = Console.ReadLine();
                int userInt = 0;
                if (userInput == "0")
                {
                    userInt = 0;
                }
                else if (userInput == "1")
                {
                    userInt = 1;
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                fileData.inputVariables[currentVariableIdx].SetValue(userInt);
                currentVariableIdx++;
            }

            Console.WriteLine();
            ComputeAndDisplay(fileData, true);
            Console.WriteLine();
        }

        static void RunAuto(FileData fileData, bool verbose)
        {
            int numInputs = fileData.inputVariables.Count;
            foreach (Variable var in fileData.inputVariables)
            {
                Console.Write(var.name + " ");
            }
            Console.Write(" | ");

            foreach (var var in fileData.localVariables)
            {
                if (var.Key.shouldOutput)
                {
                    Console.Write(var.Key.name + " ");
                }
            }
            Console.WriteLine();
            for (int i = 0; i < Math.Pow(2, numInputs); i++)
            {
                string binaryStr = Convert.ToString(i, 2).PadLeft(numInputs, '0');

                int idx = 0;
                foreach (char c in binaryStr)
                {
                    fileData.inputVariables[idx].SetValue((int)Char.GetNumericValue(c));
                    idx++;
                }

                if (verbose)
                {
                    Console.WriteLine();

                    Console.Write("Current input variables: ");
                    foreach (Variable var in fileData.inputVariables)
                    {
                        Console.Write(var.name + ": " + var.value + "   ");

                    }
                    Console.WriteLine();
                    ComputeAndDisplay(fileData, verbose);
                    Console.WriteLine();
                }
                else
                {
                    foreach (Variable var in fileData.inputVariables)
                    {
                        Console.Write(var.value + " ");
                    }
                    Console.Write(" | ");
                    ComputeAndDisplay(fileData, verbose);
                    Console.WriteLine();
                }  
                
            }
            
        }

        static void ComputeAndDisplay(FileData fileData, bool verbose)
        {
            Computer computer = new Computer(fileData);
            List<Variable> simOutput = computer.ComputeCircuit();

            foreach (Variable var in simOutput)
            {
                if (var.shouldOutput)
                {
                    if (verbose)
                    {
                        Console.Write("Output variable " + var.name + ": " + var.value + "  ");
                    }
                    else
                    {
                        Console.Write(var.value + "  ");
                    }
                }
            }
        }
    }
}