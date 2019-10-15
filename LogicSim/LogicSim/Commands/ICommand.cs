using System;
namespace LogicSim
{
    public interface ICommand
    {
        int Evaluate();
        int ExpectedNumArguments();
        void SetVariableValues(Variable[] variableValues);
        Variable[] GetVariables();
        string ToString();
    }
}
