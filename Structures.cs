using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogicSim
{
    // Represents a complete circuit in memory
    public class Circuit
    {
        // The circuit is stored in a SortedList, with an index as the key and a combination as the value
        private SortedList<int, Combination> combinations = new SortedList<int, Combination>();
        public int NumInputs => combinations[0].NumInputs;
        public int NumOutputs => combinations[0].NumOutputs;

        public void AddCombination(int position, Combination newCombo)
        {
            combinations.Add(position, newCombo);
        }

        public string GetTitleLine()
        {
            return combinations[0].GetTitleLine();
        }

        public string GetEntireLine(int idx)
        {
            return combinations[idx].ToString();
        }

        /// <summary>
        /// Prints each combination in the circuit after a short delay
        /// </summary>
        /// <param name="delay">Delay time in ms</param>
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

        /// <summary>
        /// Print the local variables associated with a particular set of inputs
        /// </summary>
        /// <param name="inputCombo">A set of input variables with values pre-set</param>
        public void PrintIndividualComboOutput(List<Variable> inputCombo)
        {
            int idx = 0;
            foreach (Combination combo in combinations.Values)
            {
                bool goodSoFar = true;
                for (int i = 0; i < inputCombo.Count; i++)
                {
                    if (inputCombo[i].Name != combo.GetInputs()[i].Name ||
                        inputCombo[i].Value != combo.GetInputs()[i].Value)
                    {
                        goodSoFar = false;
                        break;
                    }
                }

                if (goodSoFar)
                {
                    Console.WriteLine(combo.GetOutputsFormatted());
                    break;
                }

                idx++;
            }
        }
        
        public void PrintCombosWithOutput(List<Variable> outputCombo)
        {
            Console.WriteLine(GetTitleLine());
            int idx = 0;
            foreach (Combination localCombo in combinations.Values)
            {
                bool goodSoFar = true;
                for (int i = 0; i < outputCombo.Count; i++)
                {
                    if (outputCombo[i].Name != localCombo.GetInputs()[i].Name ||
                        outputCombo[i].Value != localCombo.GetInputs()[i].Value)
                    {
                        goodSoFar = false;
                        break;
                    }
                }

                if (goodSoFar)
                {
                    Console.WriteLine(localCombo);
                    break;
                }

                idx++;
            }
        }

        public List<Variable> GetInputVariables()
        {
            // each combination maintains a list of the input variables, so 0 is arbitrary
            return combinations[0].GetInputs();
        }

        /// <summary>
        /// Returns the title line (variable names), followed by each combination
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine();
            sb.Append(combinations[0].GetTitleLine() + "\n");
            foreach (int i in combinations.Keys)
            {
                sb.Append(combinations[i] + "\n");
            }

            return sb.ToString();
        }

        public List<Variable> GetOutputs()
        {
            return combinations[0].GetLocals();
        }
    }

    // A combination is a list of input variables and a list of the associated local variables for those inputs
    public class Combination
    {
        private readonly List<Variable> inputs;
        private readonly List<Variable> locals;

        public int NumInputs => inputs.Count;

        public int NumOutputs => locals.Count;

        public Combination(List<Variable> inputs, List<Variable> locals)
        {
            this.inputs = inputs;
            this.locals = locals;
        }

        public List<Variable> GetInputs()
        {
            return inputs;
        }

        /// <summary>
        /// Returns the title line, containing input variable names and local variable names in the form inputs | local
        /// ex. A B C | w1 w2
        /// </summary>
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
                if (localv.type == VariableType.OUTPUT)
                {
                    sb.Append(localv.Name + " ");
                }
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// Returns each local variable and it's value in  name: value  format, single line
        /// ex. A: 0  B:1  C: 0
        /// </summary>
        public string GetOutputsFormatted()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable v in locals)
            {
                if (v.type == VariableType.OUTPUT)
                {
                    sb.Append(v.Name + ": " + v.Value + "  ");
                }
            }

            sb.Append("\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the inputs and local variables in the format inputs | local
        /// ex. 0 0 0 | 1 0 1
        /// Padding is automatically added to the left of each value to align with the variable name's rightmost char
        /// </summary>
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
                if (localv.type == VariableType.OUTPUT)
                {
                    sb.Append(' ', localv.Name.Length - 1);
                    sb.Append(localv.Value + " ");
                }
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// Two combinations are equal if they share the same input and local variables
        /// </summary>
        /// <param name="obj">The other combination</param>
        /// <returns>True/false</returns>
        public override bool Equals(object obj)
        {
            if (obj is Combination)
            {
                Combination otherCombo = (Combination) obj;
                
                for (int i = 0; i < otherCombo.NumInputs; i++)
                {
                    if (otherCombo.GetInputs()[i].Name != locals[i].Name ||
                        otherCombo.GetInputs()[i].Value != locals[i].Value)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < otherCombo.NumOutputs; i++)
                {
                    if (otherCombo.GetLocals()[i].Name != locals[i].Name ||
                        otherCombo.GetLocals()[i].Value != locals[i].Value)
                    {
                        return false;
                    }
                }


                return true;
            }

            return false;
        }

        public List<Variable> GetLocals()
        {
            return locals;
        }

        /// <summary>
        /// Not worried about hash codes, but I probably should be
        /// </summary>
        /// <returns>200. That was the first number to come to mind</returns>
        public override int GetHashCode()
        {
            return 200;
        }
    }

    // The different circuit objects maintained by the program are stored here
    public static class CircuitGroup
    {
        public static readonly Circuit mainCircuit = new Circuit();
    }
}