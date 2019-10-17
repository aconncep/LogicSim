using System;
namespace LogicSim.Commands
{
    public class NOT : Command
    {
        public override Variable[] Variables { get; } = new Variable[1];
        
        public override int Evaluate()
        {
           Variables[0].FlipValue();
           return Variables[0].value;
        }

        public override int ExpectedNumArguments()
        {
            return 1;
        }
        
        public override string ToString()
        {
            return "NOT";
        }
    }
}