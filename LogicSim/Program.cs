using System;
using System.Collections.Generic;
using LogicSim.Commands;

namespace LogicSim
{
    class Program
    {
        static void Main(string[] args)
        {
            
            FileReader reader = new FileReader("test.txt");

            FileData fileData = reader.GenerateFileData();

            int idx = 0;
            foreach (Variable var in fileData.inputVariables)
            {
                Console.WriteLine("Enter value for variable " + var.name + ":");
                fileData.inputVariables[idx].SetValue(Convert.ToInt16(Console.ReadLine()));
                idx++;
            }
            
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
