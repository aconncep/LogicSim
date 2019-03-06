using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LogicSim
{
    /// <summary>
    /// This class is responsible for computing the variable value of every local variable in the circuit
    /// given an array of input variables with pre-set values
    /// </summary>
    public class Computer
    {
        readonly Dictionary<Variable, ICommand> localVariables;
        readonly List<Variable> inputVariables; 
        List<Variable> allArgVars = new List<Variable>();

        public Computer(FileData fd)
        {
            localVariables = fd.localVariables;
            inputVariables = fd.inputVariables;
        }

        public List<Variable> ComputeCircuit()
        {
            initAllArgVars();
            initAllInputVars();
            ComputeLocalVars();
            List<Variable> allVariablesOutput = new List<Variable>();
            foreach (Variable var in inputVariables)
            {
                allVariablesOutput.Add(var);
            }
            foreach (Variable var in localVariables.Keys)
            {
                allVariablesOutput.Add(var);
            }
            return allVariablesOutput;

        }

        private void initAllInputVars()
        {
            foreach (Variable var in allArgVars)
            {
                foreach (Variable inputVar in inputVariables)
                {
                    if (var.Equals(inputVar))
                    {
                        var.SetValue(inputVar.value);
                    }
                }
                
            }
        }
        

        private void initAllArgVars()
        {
            foreach (ICommand command in localVariables.Values)
            {
                foreach (Variable var in command.GetVariables())
                {
                    Console.WriteLine(var.GetHashCode()); // debug purposes
                    allArgVars.Add(var);
                }
                
            }
        }
        private void ComputeLocalVars() 
        {
            foreach (Variable var in localVariables.Keys)
            {                
                if (localVariables.TryGetValue(var, out ICommand command))
                {
                    int resultFromAssociatedCommand = command.Evaluate();
                    var.SetValue(resultFromAssociatedCommand);
                   

                    foreach (var a in allArgVars)
                    {
                        if (a.Equals(var))
                        {
                            a.SetValue(var.value);
                        }
                    }
  
                }        
            }      
        }
    }
}
