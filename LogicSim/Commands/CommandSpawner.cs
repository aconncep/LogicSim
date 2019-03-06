namespace LogicSim.Commands
{
    /// <summary>
    /// Takes the string representation of a command and creates a new command object of that command type
    /// (for example, this can turn the string "NOT" into a new NOT object)
    /// NOTE: This returns a blank command. It's variables need to be set manually
    /// </summary>
    public static class CommandSpawner
    {
        public static ICommand GetCommandObjectFromString(string commandString, int lineNumber)
        {
            switch (commandString)
            {
                case "NOT":
                    return new NOT();
                
                case "AND":
                    return new AND();
                
                default: // if the users desired command is not valid
                    throw new CompilationException("[" + commandString + "] is an invalid command", lineNumber);
            }
        }

    }
}