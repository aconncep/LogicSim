namespace LogicSim
{
    public static class StringUtilities
    {
        public static string TrimVariableAssignment(string line)
        {
            int equalsIndex = line.IndexOf('=');
            return line.Substring(equalsIndex+1, (line.Length-1)-equalsIndex).Trim();
        }
        
        public static string RemoveInlineComments(string line)
        {
            if (line.Contains('#'))
            {
                line = line.Substring(0, line.IndexOf('#'));
                if (line.EndsWith(' '))
                {
                    line = line.Substring(0, line.Length - 1);
                    return line;
                }
                return line;
            }
            return line;
        }
        
        public static string DetermineOuterCommand(string line)
        {
            int openParenthesisIndex = line.IndexOf('(');
            return line.Substring(0, openParenthesisIndex);
        }
        
        
        public static int FindThisCommandsComma(string line)
        {
            int parenthesisCount = 0;
            int idx = 0;

            bool hasSeenFirst = false;

            foreach (char c in line)
            {
                idx++;
                if (c == '(')
                {
                    parenthesisCount++;
                    if (hasSeenFirst == false)
                    {
                        hasSeenFirst = true;
                    }
                }

                if (c == ')')
                {
                    parenthesisCount--; 
                }
                

                if (parenthesisCount == 0 && hasSeenFirst)
                {
                    break;
                }
                
            }

            return  idx;

        }
    }
}