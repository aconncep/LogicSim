using System;
using System.IO;
using System.Collections.Generic;

namespace LogicSim
{
    class Program
    {
        public static string[] FileLines { get; private set; }
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
                    RunModes.RunAuto(false);
                }
                else if (CompareArgStrings(args[1], "auto") &&
                         CompareArgStrings(args[2], "verbose"))
                {
                    RunModes.RunAuto(true);
                }
                else if (CompareArgStrings(args[1], "manual") &&
                         CompareArgStrings(args[2], "simple"))
                {
                    Console.WriteLine("Manual simple mode is not supported. Running manual verbose instead...");
                    RunModes.RunManual(true);
                }
                else if (CompareArgStrings(args[1], "manual") &&
                         CompareArgStrings(args[2], "verbose"))
                {
                    RunModes.RunManual(true);
                }
                else
                {
                    RunWithoutArgs();
                }
            }
        }

        static void RunWithoutArgs()
        {
            if (FileLines == null)
            {
                Console.WriteLine("Usage: LogicSim [path_to_file] [auto/manual] [verbose/simple]");
                Environment.Exit(1);
            }

            string[] selections = new string[2];

            while (true)
            {
                Console.WriteLine("Select a run mode:");
                Console.WriteLine("1. Auto");
                Console.WriteLine("2. Manual");

                char userIn = Console.ReadKey().KeyChar;
                Console.WriteLine();
                
                if (userIn == '1')
                {
                    selections[0] = "auto";
                    break;
                }
                else if (userIn == '2')
                {
                    selections[0] = "manual";
                    break;
                }

                Console.WriteLine("Invalid input. Try again.");
                Console.WriteLine();
            }

            if (selections[0] == "auto")
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
                        selections[1] = "verbose";
                        break;
                    }
                    else if (userIn == '2')
                    {
                        selections[1] = "simple";
                        break;
                    }

                    Console.WriteLine("Invalid input. Try again.");
                    Console.WriteLine();
                }
            }
            else // we must use manual verbose, manual simple is not possible
            {
                RunModes.RunManual(true);
            }

            if (selections[0] == "auto" && selections[1] == "simple")
            {
                RunModes.RunAuto(false);
            }
            else if (selections[0] == "auto" && selections[1] == "verbose")
            {
                RunModes.RunAuto(true);
            }
        }

        static bool CompareArgStrings(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }
        
        

    }
}