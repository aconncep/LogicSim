using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogicSim
{
    public class Circuit
    {
        private SortedList<int, Combination> combinations = new SortedList<int, Combination>();
        public int NumInputs => combinations[0].NumInputs;
        public int NumOutputs => combinations[0].NumOutputs;

        public void AddCombination(int position, Combination newCombo)
        {
            combinations.Add(position, newCombo);
        }

        public void PrintWithDelay(int delay)
        {
            Console.WriteLine();
            Console.WriteLine(combinations[0].GetTitleLine());
            foreach (int i in combinations.Keys)
            {
                Thread.Sleep(delay);
                Console.WriteLine(combinations[i]);
            }
        }

        public void PrintIndividualComboOutput(List<Variable> inputCombo)
        {
            int idx = 0;
            foreach (Combination combo in combinations.Values)
            {
                if (combo.GetInputs().SequenceEqual(inputCombo))
                {
                    Console.WriteLine(combinations[idx].GetLocalsFormatted());
                    break;
                }
                idx++;
            }
            
            // invalid input
        }

        public List<Variable> GetInputVariables()
        {
            return combinations[0].GetInputs();
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine();
            sb.Append(combinations[0].GetTitleLine()+"\n");
            foreach (int i in combinations.Keys)
            {
                sb.Append(combinations[i] + "\n");
            }

            return sb.ToString();
        }
    }

    public class Combination
    {
        private readonly List<Variable> inputs;
        private readonly List<Variable> locals;

        public int NumInputs => inputs.Count;

        public int NumOutputs => locals.Count;

        public Combination()
        {
            inputs = new List<Variable>();
            locals = new List<Variable>();
        }

        public void AddInput(Variable newVar)
        {
            if (newVar != null)
            {
                inputs.Add(newVar);
            }
        }
        public Combination(List<Variable> inputs, List<Variable> locals)
        {
            this.inputs = inputs;
            this.locals = locals;
        }

        public List<Variable> GetInputs()
        {
            return inputs;
        }
        
        public List<Variable> GetLocals()
        {
            return locals;
        }
        
        public string GetTitleLine()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable inputv in inputs)
            {
                sb.Append(inputv.Name + " ");
            }

            sb.Remove(sb.Length - 1, 1);

            sb.Append(" | ");
            foreach (Variable localv in locals)
            {
                sb.Append(localv.Name + " ");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public string GetLocalsFormatted()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable v in locals)
            {
                sb.Append(v.Name + ": " + v.Value + "  ");
            }

            sb.Append("\n");
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable inputv in inputs)
            {
                sb.Append(' ', inputv.Name.Length - 1);
                sb.Append(inputv.Value + " ");
            }
            
            sb.Remove(sb.Length - 1, 1);
            
            sb.Append(" | ");
            foreach (Variable localv in locals)
            {
                sb.Append(' ', localv.Name.Length - 1);
                sb.Append(localv.Value + " ");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Combination)
            {
                Combination otherCombo = (Combination) obj;
                return otherCombo.inputs == inputs && otherCombo.locals == locals;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 200;
        }
    }

    static class CircuitGroup
    {
        public static readonly Circuit mainCircuit = new Circuit();
    }

}