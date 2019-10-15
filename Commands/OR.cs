using System;
namespace LogicSim.Commands
{
    public class OR : ICommand
    {
        Variable[] variables;

        public OR() { }

        public int ExpectedNumArguments()
        {
            return 2;
        }

        public void SetVariableValues(Variable[] vars)
        {
            variables = new Variable[] { vars[0], vars[1] };
        }

        public int Evaluate()
        {
            if (variables[0].value == 1 || variables[1].value == 1)
            {
                return 1;
            }
            return 0;
        }

        public Variable[] GetVariables()
        {
            return variables;
        }

        public override string ToString()
        {
            return "OR";
        }
    }
}
