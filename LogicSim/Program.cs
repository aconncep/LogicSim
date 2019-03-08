using System;
using System.IO;
using System.Collections.Generic;

namespace LogicSim
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                FileReader reader = null;
                try
                {
                    reader = new FileReader(args[0]);  
                    BREAKINGTHEBUILD
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
                    RunAuto(fileData);
                }
                else if (String.Equals(args[1], "manual", StringComparison.Ordinal))
                {
                    RunManual(fileData);
                }
                else
                {
                    Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual]");
                }
            }
            else
            {
                Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual]");
            }
        }

        static void RunManual(FileData fileData)
        {
            int idx = 0;
            foreach (Variable var in fileData.inputVariables)
            {
                Console.WriteLine("Enter value for variable " + var.name + ":");
                fileData.inputVariables[idx].SetValue(Convert.ToInt16(Console.ReadLine()));
                idx++;
            }
            
            ComputeAndDisplay(fileData);
            Console.WriteLine();
        }

        static void RunAuto(FileData fileData)
        {
            int numInputs = fileData.inputVariables.Count;
            for (int i = 0; i < Math.Pow(2, numInputs); i++)
            {
                string binaryStr = Convert.ToString(i, 2).PadLeft(numInputs, '0');

                int idx = 0;
                foreach (char c in binaryStr)
                {
                    fileData.inputVariables[idx].SetValue((int)Char.GetNumericValue(c));
                    idx++;
                }

               
                Console.Write("Current variables: ");
                foreach (Variable var in fileData.inputVariables)
                {
                    Console.Write(var.name + ": " + var.value + "   " );
                    
                }
                Console.WriteLine();
                ComputeAndDisplay(fileData);
                Console.WriteLine();
            }
            
        }

        static void ComputeAndDisplay(FileData fileData)
        {
            Computer computer = new Computer(fileData);
            List<Variable> simOutput = computer.ComputeCircuit();

            foreach (Variable var in simOutput)
            {
                if (var.shouldOutput)
                {
                    Console.WriteLine(var);               
                }
            }
        }
    }
}