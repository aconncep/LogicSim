namespace LogicSim
{
    public class Commands
    {
        public static int AND(int var1, int var2)
        {
            if (var1 == 0 || var2 == 0)
            {
                return 0;
            }

            return 1;
        }
        
        public static int OR(int var1, int var2)
        {
            if (var1 == 1 || var2 == 1)
            {
                return 1;
            }

            return 0;
        }
        
    }
}