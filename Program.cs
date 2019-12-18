﻿using System;
using System.IO;
using System.Linq;

namespace LogicSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LogicSim v3.0 by Austin Cepalia\n");
            Console.WriteLine("Looking for HDL file...");
            
            if (args.Length > 0)
            {
                try
                {
                    string[] lines = File.ReadAllLines(args[0]);
                    Console.WriteLine("HDL file detected.");
                    RunModes.GenerateCircuit(lines);
                    Console.WriteLine($"Successfully processed file {args[0]} with {lines.Length} lines.\n");
                    RunFullMenu();
                }
                catch (InterpreterException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to process specified HDL file. Check file path and file contents, then run again.");
                    Console.WriteLine("Some program functionality will be unavailable.\n");
                    RunShortMenu();
                }
            }
            else
            {
                Console.WriteLine("HDL file not detected. Some program functionality will be unavailable.\n");
                RunShortMenu();
                
            }
        }

        /// <summary>
        /// Display the full menu and run the method relating to the user's choice.
        /// The full menu is shown when the software successfully generates a circuit object out of the HDL file
        /// </summary>
        static void RunFullMenu()
        {
            while (true)
            {
                var userSelection = MenuSelection.PromptNoQuestion(menuWithFile, false);
                if (userSelection is Selection && userSelection == Selection.QUIT)
                {
                    Environment.Exit(0);
                }

                switch (userSelection)
                {
                    case -1:
                        Console.WriteLine();
                        RunModes.RunAutoNoDelay();
                        break;
                    
                    case 0:
                        Console.WriteLine();
                        RunModes.RunAuto();
                        break;
                    
                    case 1:
                        Console.WriteLine();
                        RunModes.RunManual();
                        break;
                    
                    case 2:
                        Console.WriteLine();
                        RunModes.StepThroughInputs();
                        break;
                    
                    case 4:
                        Console.WriteLine();
                        RunModes.ShowOnlyOutput(0);
                        break;
                    
                    case 5:
                        Console.WriteLine();
                        RunModes.ShowOnlyOutput(1);
                        break;
                        
                    case 14:
                        Console.WriteLine();
                        Help();
                        break;
                    case 15:
                        Console.WriteLine();
                        About();
                        break;
                }
            }
        }

        /// <summary>
        /// Display the short menu and run the method relating to the user's choice.
        /// The short menu is shown when the software cannot generate a circuit object out of the HDL file
        /// </summary>
        static void RunShortMenu()
        {
            while (true)
            {
                var userSelection = MenuSelection.PromptNoQuestion(menuWithoutFile, true);
                if (userSelection is Selection && userSelection == Selection.QUIT)
                {
                    Environment.Exit(0);
                } 
                
                switch (userSelection)
                {
                    case 4:
                        Console.WriteLine();
                        Help();
                        break;
                    case 5:
                        Console.WriteLine();
                        About();
                        break;
                }
            }
        }

        /// <summary>
        /// Prompt the user with all the functionality they can get more info about
        /// This is the entire fullMenu, but without the last two options 
        /// </summary>
        static void Help()
        {
            while (true)
            {
                var userSelection = MenuSelection.PromptWithQuestion("Which program functionality would you like to learn more about?",
                    menuWithFile.Take(menuWithFile.Length-2).ToArray(), false);
                if (userSelection is Selection && userSelection == Selection.QUIT)
                {
                    Console.WriteLine("Returning to the main menu...\n");
                    break;
                }

                Console.WriteLine();

                switch (userSelection)
                {
                    case 0:
                        HelpMenus.HelpSimulateAuto();
                        break;
                    case 1:
                        HelpMenus.HelpSimulateManual();
                        break;
                    case 2:
                        HelpMenus.HelpStepInputs();
                        break;
                    case 3:
                        HelpMenus.HelpStepCircuit();
                        break;
                    case 4:
                        HelpMenus.HelpShowZero();
                        break;
                    case 5:
                        HelpMenus.HelpShowOne();
                        break;
                    case 6:
                        HelpMenus.HelpHoldInput();
                        break;
                    case 7:
                        HelpMenus.HelpEquivalence();
                        break;
                    case 8:
                        HelpMenus.HelpReplace();
                        break;
                    case 9:
                        HelpMenus.HelpLearn();
                        break;
                    case 10:
                        HelpMenus.HelpQuiz();
                        break;
                    case 11:
                        HelpMenus.HelpTruth();
                        break;
                    case 12:
                        HelpMenus.HelpClassroom();
                        break;
                    case 13:
                        HelpMenus.HelpReinterpret();
                        break;
                }

                Console.WriteLine("\nPress any key to return to the Help menu...\n");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// The about menu
        /// </summary>
        static void About()
        {
            Console.WriteLine("LogicSim is a cross-platform tool for simulating combinational logic circuits. It can be used to quickly generate " +
                              "circuit output for circuits of virtually unlimited complexity, with virtually unlimited inputs.\nLogicSim supports the 7 major logic operations (called COMMANDS), " +
                              "including NOT, AND, OR, XOR, NAND, NOR, and XNOR.\nThese commands are entered in a Hardware Description Language (HDL) file, which the program will interpret if it is supplied " +
                              "as a command-line argument at runtime.\n" +
                              "Additionally, the software has tools to help with learning combinational logic, including a game mode that allows a classroom of students to work together to defeat a monster " +
                              "by solving combinational logic circuits.\n" +
                              "An early version of LogicSim has been used to teach combinational logic at a kids summer camp at Rochester Institute of Technology.\n" +
                              "LogicSim started as a personal project by CS student Austin Cepalia his freshman year of college and has grown with him ever since.\n");
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadLine();
        }

        // A string array representing the full menu
        private static string[] menuWithFile =
        {
            "Simulate circuit (Auto)",
            "Simulate circuit (Manual)",
            "Step through inputs",
            "Step through circuit",
            "Show only 0 outputs",
            "Show only 1 outputs",
            "Hold input constant",
            "Equivalence classes",
            "Replace command",
            "Learn combinational logic",
            "Quiz",
            "Truth tables (reference)",
            "Classroom Battle!",
            "Re-interpret HDL File",
            "Help",
            "About"
        };
        
        // A string array representing the shortened menu
        private static string[] menuWithoutFile =
        {
            "Learn combinational logic",
            "Quiz",
            "Truth tables (reference)",
            "Classroom Battle!",
            "Help",
            "About"
        };

    }
}