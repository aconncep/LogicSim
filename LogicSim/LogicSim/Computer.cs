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
            InitAllArgVars();
            InitAllInputVars();
            ComputeLocalVars();
            
            // Create a list of every variables in it's final form
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
        
        // fill allArgVars list with all the variables found in command arguments
        private void InitAllArgVars()
        {
            foreach (ICommand command in localVariables.Values)
            {
                foreach (Variable var in command.GetVariables())
                {
                    allArgVars.Add(var);
                }
                
            }
        }

        // set the value of any allArgVars variable that is an input (A,B,C...)
        private void InitAllInputVars()
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
        
        // compute the value of each local variable (w1,w2,w3...)
        private void ComputeLocalVars() 
        {
            foreach (Variable var in localVariables.Keys)
            {
                // try to get the associated command with each value
                if (localVariables.TryGetValue(var, out ICommand command))
                {
                    var.SetValue(command.Evaluate());
                    
                    // if this command contains the local var we're working with, update it's value
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
