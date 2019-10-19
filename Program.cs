using System;
using System.IO;
using System.Collections.Generic;

namespace LogicSim
{
    class Program
    {
        private static string[] FileLines { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("LogicSim v2.0 by Austin Cepalia");
            
            // Try to open the file to make sure it exists
            try
            {
                FileLines = File.ReadAllLines(args[0]);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Unable to open file " + args[0]);
                Environment.Exit(1);
            } 

            // if the wrong number of arguments are provided, ask user for them in console
            if (args.Length != 3)
            {
                RunWithoutArgs();
            }
            else
            {
                // Determine the run configuration
                if (!CompareArgStrings(args[1], "auto") &&
                        !CompareArgStrings(args[1], "manual") &&
                        !CompareArgStrings(args[2], "simple") &&
                        !CompareArgStrings(args[2], "verbose"))
                {
                    RunWithoutArgs();
                }
                else if (CompareArgStrings(args[1], "auto") &&
                         CompareArgStrings(args[2], "simple"))
                {
                    RunModes.RunAuto(false, FileLines);
                }
                else if (CompareArgStrings(args[1], "auto") &&
                         CompareArgStrings(args[2], "verbose"))
                {
                    RunModes.RunAuto(true, FileLines);
                }
                else if (CompareArgStrings(args[1], "manual") &&
                         CompareArgStrings(args[2], "simple"))
                {
                    Console.WriteLine("Manual simple mode is not supported. Running manual verbose instead...");
                    RunModes.RunManual(true, FileLines);
                }
                else if (CompareArgStrings(args[1], "manual") &&
                         CompareArgStrings(args[2], "verbose"))
                {
                    RunModes.RunManual(true, FileLines);
                }
                else
                {
                    RunWithoutArgs();
                }
            }
        }

        /// <summary>
        /// If the wrong number of arguments is supplied or an invalid argument is specified
        /// This is like the fail-safe, but it still requires a valid file
        /// </summary>
        static void RunWithoutArgs()
        {
            if (FileLines == null)
            {
                Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
                Environment.Exit(1);
            }

            // array of RunLabels to store user inputs
            RunLabel[] selections = new RunLabel[2];

            while (true)
            {
                Console.WriteLine("Select a run mode:");
                Console.WriteLine("1. Auto");
                Console.WriteLine("2. Manual");

                char userIn = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                if (userIn == '1')
                {
                    selections[0] = RunLabel.AUTO;
                    break;
                }
                else if (userIn == '2')
                {
                    selections[0] = RunLabel.MANUAL;
                    break;
                }

                Console.WriteLine("Invalid input. Try again.");
                Console.WriteLine();
            }

            if (selections[0] == RunLabel.AUTO)
            {
                while (true)
                {
                    Console.WriteLine("Select a verbosity mode:");
                    Console.WriteLine("1. Verbose");
                    Console.WriteLine("2. Simple");
                    
                    char userIn = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    
                    if (userIn == '1')
                    {
                        selections[1] = RunLabel.VERBOSE;
                        break;
                    }
                    else if (userIn == '2')
                    {
                        selections[1] = RunLabel.SIMPLE;
                        break;
                    }

                    Console.WriteLine("Invalid input. Try again.");
                    Console.WriteLine();
                }
            }
            else // we must use manual verbose, manual simple is not possible
            {
                RunModes.RunManual(true, FileLines);
            }

            // determine the run mode from the entered selections
            if (selections[0] == RunLabel.AUTO && selections[1] == RunLabel.SIMPLE)
            {
                RunModes.RunAuto(false, FileLines);
            }
            else if (selections[0] == RunLabel.AUTO && selections[1] == RunLabel.VERBOSE)
            {
                RunModes.RunAuto(true, FileLines);
            }
        }

        static bool CompareArgStrings(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }
        
        

    }
}