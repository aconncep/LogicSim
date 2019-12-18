using System;

namespace LogicSim
{
    public enum Selection
    {
        TRUE,FALSE,QUIT
    }
    
    
    public static class MenuSelection
    {
        public static dynamic PromptWithQuestion(string question, string[] selections, bool immediate=true)
        {
            if (selections.Length > 10)
            {
                immediate = false;
            }
            
            while (true)
            {
                if (question != null)
                {
                    Console.WriteLine(question);
                    
                }
                Console.WriteLine("Select an option below or press [x] to quit");
                for (int i = 0; i < selections.Length; i++)
                {
                    Console.WriteLine(i + ". " + selections[i]);
                }
                int selection;
                
                if (immediate)
                {
                    ConsoleKeyInfo userIn = Console.ReadKey();

                    if (userIn.Key == ConsoleKey.X)
                    {
                        Console.WriteLine();
                        return Selection.QUIT;
                    }

                    if (char.IsDigit(userIn.KeyChar))
                    {
                        selection = int.Parse(userIn.KeyChar.ToString());
                        if (selection >= 0 && selection < selections.Length)
                        {
                            Console.WriteLine();
                            return selection;
                        }
                    }
                }
                else
                {
                    string userIn = Console.ReadLine();
                    if (userIn == "x" || userIn == "X")
                    {
                        Console.WriteLine();
                        return Selection.QUIT;
                    }

                    bool isNumeric = int.TryParse(userIn, out selection);

                    if (isNumeric)
                    {
                        if (selection >= -1 && selection < selections.Length)
                        {
                            return selection;
                        }
                    }
                }
                Console.WriteLine("\nInvalid selection. Try again\n");

            }
        }

        public static dynamic PromptNoQuestion(string[] selections, bool immediate=true)
        {
            return PromptWithQuestion(null, selections, immediate);
        }
        public static Selection PromptTrueFalse(string question)
        {
            
            while (true)
            {
                Console.WriteLine(question + " [T/F/x]");

                ConsoleKeyInfo userIn = Console.ReadKey();
                
                switch (userIn.Key)
                {
                    case ConsoleKey.X:
                        return Selection.QUIT;
                    case ConsoleKey.T:
                        return Selection.TRUE;
                    case ConsoleKey.F:
                        return Selection.FALSE;
                    default:
                        Console.WriteLine("\nInvalid selection. Try again\n");
                        break;
                }
            }
            
        }
        
    }
}