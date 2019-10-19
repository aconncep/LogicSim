using System;

namespace LogicSim
{
    public enum RunLabel
    {
        AUTO,MANUAL,SIMPLE,VERBOSE
    }
    
    public class RunModes
    {
        public static void RunAuto(bool verbose, string[] fileLines)
        {
            Console.WriteLine("Running in auto, verbose is " + verbose);
            Interpreter.Interpret(fileLines);
        }

        public static void RunManual(bool verbose, string[] fileLines)
        {
            Console.WriteLine("Running in manual, verbose is " + verbose);
        }
    }
}