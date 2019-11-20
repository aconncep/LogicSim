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
            Console.WriteLine();
            try
            {
                Interpreter.Interpret(fileLines, true, verbose);
            }
            catch (InterpreterException e)
            {
                Console.WriteLine(e.Message);
            }
            //catch (SystemException e)
            //{
                //Console.WriteLine("Something went wrong. Check your HDL file and try again.");
            //}
        }

        public static void RunManual(bool verbose, string[] fileLines)
        {
            Console.WriteLine("Running in manual, verbose is " + verbose);
            Console.WriteLine();
            try
            {
                Interpreter.Interpret(fileLines, false, verbose);
            }
            catch (InterpreterException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SystemException e)
            {
                Console.WriteLine("Something went wrong. Check your HDL file and try again.");
            }
        }
    }
}