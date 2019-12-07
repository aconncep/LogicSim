using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LogicSim
{
    public class Circuit
    {
        
        private Dictionary<Combination, Combination> inputToLocal = new Dictionary<Combination, Combination>();

        public int NumberInputs;


        public void AddCombination(Combination inputCombo, Combination outputCombo)
        {
            inputToLocal.Add(inputCombo, outputCombo);
        }

        public Combination GetOutputForInput(Combination inputCombo)
        {
            return inputToLocal[inputCombo];
        }

        public Combination GetInputVariables()
        {
            return inputToLocal.ElementAt(0).Key;
        }


        public int NumberCombos()
        {
            return Convert.ToInt16(Math.Pow(2, NumberInputs));
        }

        public void PrintHeader()
        {
            StringBuilder topLine = new StringBuilder();
            int idx = 0;
            foreach (KeyValuePair<Combination, Combination> combo in inputToLocal)
            {
                if (idx == 0)
                {
                    foreach (Variable v in combo.Key.combination)
                    {
                        if (v.type == VariableType.INPUT)
                        {
                            topLine.Append(v.Name + " ");
                        }
                    }

                    topLine.Append(" | ");

                    foreach (Variable v in combo.Value.combination)
                    {
                        if (v.type == VariableType.LOCAL)
                        {
                            topLine.Append(v.Name + " ");
                        }
                    }
                }
            }

            Console.WriteLine(topLine);
        }
        
        
        public void Print()
        {
            StringBuilder topLine = new StringBuilder();
            int idx = 0;
            foreach (KeyValuePair<Combination,Combination> combo in inputToLocal)
            {
                if (idx == 0)
                {
                    foreach (Variable v in combo.Key.combination)
                    {
                        if (v.type == VariableType.INPUT)
                        {
                            topLine.Append(v.Name + " ");
                        }
                    }
                    
                    topLine.Append(" | ");
                    
                    foreach (Variable v in combo.Value.combination)
                    {
                        if (v.type == VariableType.LOCAL)
                        {
                            topLine.Append(v.Name + " ");
                        }
                    }
                }
                

                topLine.Append('\n');

                foreach (Variable v in combo.Key.combination)
                {
                    if (v.type == VariableType.INPUT)
                    {
                        topLine.Append(v.Value + " ");
                    }
                }
                
                topLine.Append(" | ");
                
                foreach (Variable v in combo.Value.combination)
                {
                    if (v.type == VariableType.LOCAL)
                    {
                        topLine.Append(v.Value + " ");
                    }
                }
                idx++;
            }

            Console.WriteLine(topLine + "\n");
        }
    }
            
    public class CircuitGroup
    {
        // the main circuit to be computed
        public Circuit mainCircuit = new Circuit();
        
        // When the user modifies the circuit in-program, represent it here
        public Circuit modifiedCircuit = new Circuit();
    }

    public class Combination
    {
        public List<Variable> combination;

        public Combination()
        {
            combination = new List<Variable>();
        }
        public Combination(List<Variable> input)
        {
            combination = input;
        }

        public void SetVariable(string name, int value)
        {
            if (combination.Contains(new Variable(name, value, VariableType.INPUT)))
            {
                foreach (Variable v in combination)
                {
                    if (v.Name == name)
                    {
                        v.Value = value;
                    }
                }
            }
            else
            {
                combination.Add(new Variable(name, value, VariableType.INPUT));
            }
            
        }

        public override bool Equals(object obj)
        {
            if (obj is Combination)
            {
                Combination otherCombo = (Combination) obj;

                if (otherCombo.combination.Count != combination.Count)
                {
                    return false;
                }

                for (int idx = 0; idx < combination.Count; idx++)
                {
                    if (!otherCombo.combination[idx].Equals(combination[idx]))
                    {
                        return false;
                    }

                }
                
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 100;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach (Variable v in combination)
            {
                b.Append(v.Name + ": " + v.Value + "   ");
            }

            return b.ToString();
        }
    }
}