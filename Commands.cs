namespace LogicSim
{
    public class Commands
    {
        private static int FlipBit(int bit)
        {
            if (bit == 0)
            {
                return 1;
            }

            return 0;
        }

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

        public static int NOT(int var1)
        {
            return FlipBit(var1);
        }

        public static int NOR(int var1, int var2)
        {
            return FlipBit(OR(var1, var2));
        }
        
        public static int NAND(int var1, int var2)
        {
            return FlipBit(AND(var1, var2));
        }

        public static int XNOR(int var1, int var2)
        {
          if ((var1 == 1 && var2 == 1) || (var1 == 0 && var2 == 0))
          {
              return 1;
          }

          return 0;
        }
        
        public static int XOR(int var1, int var2)
        {
            if (var1 == var2)
            {
                return 0;
            }

            return 1;
        }
        
        
        
    }
}