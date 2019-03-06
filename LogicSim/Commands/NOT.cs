using System;
namespace LogicSim.Commands
{
    public class NOT : ICommand
    {
        Variable[] variables;

        public NOT() { }

        public void SetVariableValues(Variable[] vars)
        {
            variables = new Variable[] { vars[0] };
        }

        public int Evaluate()
        {
           return variables[0].FlipValue();
        }

        public int ExpectedNumArguments()
        {
            return 1;
        }
        
        public Variable[] GetVariables()
        {
            return variables;
        }

        public override string ToString()
        {
            return "NOT";
        }
    }
}
