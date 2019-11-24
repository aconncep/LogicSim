using System;
using System.Diagnostics;

namespace LogicSim
{
    public enum RunLabel
    {
        AUTO,MANUAL,SIMPLE,VERBOSE
    }
    
    public class RunModes
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        public static void RunAuto(bool verbose, string[] fileLines)
        {
            Console.WriteLine("Running in auto, verbose is " + verbose);
            Console.WriteLine();
            Console.Write("Enter calculation delay in milliseconds (or 0 for instantaneous): ");
            string userIn = Console.ReadLine();
            try
            {
                int delay = Int32.Parse(userIn);
                _stopwatch.Start();
                Interpreter.Interpret(fileLines, true, verbose, delay);
                _stopwatch.Stop();
                Console.WriteLine(
                    $"Calculated in {_stopwatch.ElapsedMilliseconds} ms ({_stopwatch.ElapsedMilliseconds / 1000.0} sec.)");

            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid delay time [{userIn}], restarting...");
                Console.WriteLine();
                RunAuto(verbose, fileLines);
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

        public static void RunManual(bool verbose, string[] fileLines)
        {
            Console.WriteLine("Running in manual, verbose is " + verbose);
            Console.WriteLine();
            try
            {
                Interpreter.Interpret(fileLines, false, verbose, 0);
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