using System;
namespace LogicSim.Commands
{
    public class OR : Command
    {
        
        public override int ExpectedNumArguments()
        {
            return 2;
        }

        public override int Evaluate()
        {
            if (Variables[0].value == 1 || Variables[1].value == 1)
            {
                return 1;
            }
            return 0;
        }

        public override string ToString()
        {
            return "OR";
        }
    }
}
