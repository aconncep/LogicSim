using System;
namespace LogicSim.Commands
{
    public class XOR : ICommand
    {
        Variable[] variables;

        public XOR() { }

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
            if (variables[0].value == 0 && variables[1].value == 1 || variables[0].value == 1 && variables[1].value == 0)
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
            return "XOR";
        }
    }
}
