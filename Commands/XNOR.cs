using System;
namespace LogicSim.Commands
{
    public class XNOR : Command
    {
        public override int ExpectedNumArguments()
        {
            return 2;
        }
        
        public override int Evaluate()
        {
            if (Variables[0].value == 0 && Variables[1].value == 1 || Variables[0].value == 1 && Variables[1].value == 0)
            {
                return 0;
            }
            return 1;
        }

        public Variable[] GetVariables()
        {
            return Variables;
        }

        public override string ToString()
        {
            return "XNOR";
        }
    }
}
