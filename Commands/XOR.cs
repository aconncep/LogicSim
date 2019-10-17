using System;
namespace LogicSim.Commands
{
    public class XOR : Command
    {
        public override int ExpectedNumArguments()
        {
            return 2;
        }

        public override int Evaluate()
        {
            if (Variables[0].value == 0 && Variables[1].value == 1 || Variables[0].value == 1 && Variables[1].value == 0)
            {
                return 1;
            }
            return 0;
        }

        public override string ToString()
        {
            return "XOR";
        }
    }
}
