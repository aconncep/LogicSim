using System;
namespace LogicSim.Commands
{
    public class AND : ICommand
    {
        Variable[] variables;

        public AND() { }
        public AND(Variable[] vars)
        {
            variables = new Variable[] { vars[0], vars[1] };
            
        }

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
            if (variables[0].value == 1 && variables[1].value == 1)
            {
                return 1;
            }
            return 0;
        }

        public void UpdateVariables(Variable variable)
        {
            foreach (Variable var in variables)
            {
                if (var.Equals(variable))
                {
                    var.SetValue(variable.value);
                }
            }
        }

        public Variable[] GetVariables()
        {
            return variables;
        }
        
        

        public override string ToString()
        {
            return "AND";
        }
    }
}
