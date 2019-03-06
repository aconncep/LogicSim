using System;
namespace LogicSim
{
    public interface ICommand
    {
        int Evaluate();
        int ExpectedNumArguments();
        void SetVariableValues(Variable[] variableValues);
        void UpdateVariables(Variable var);
        Variable[] GetVariables();
        string ToString();
    }
}
