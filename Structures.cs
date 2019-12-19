using System;
using System.Collections;
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

        public void ClearCombinations()
        {
            combinations.Clear();
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
            Thread.Sleep(delay);
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
            SortedList<int, Combination> matchingCombos = new SortedList<int, Combination>();
            int idx = 0;

            foreach (Combination localCombo in combinations.Values)
            {
                bool goodSoFar = true;
                for (int i = 0; i < outputCombo.Count; i++)
                {
                    List<Variable> locals = localCombo.GetLocals();
                    if (locals[i].type != VariableType.OUTPUT)
                    {
                        continue;
                    }
                    if (outputCombo[i].Name != locals[i].Name ||
                        outputCombo[i].Value != locals[i].Value)
                    {
                        goodSoFar = false;
                        break;
                    }
                }

                if (goodSoFar)
                {
                    matchingCombos.Add(idx, localCombo);
                    
                }

                idx++;
            }

            if (matchingCombos.Count == 0)
            {
                Console.WriteLine("No input combinations matched.\n");
            }
            else
            {
                Console.WriteLine(GetTitleLine());
                foreach (Combination combo in matchingCombos.Values)
                {
                    Console.WriteLine(combo);
                }

                Console.WriteLine($"\nDisplaying {matchingCombos.Count} input combinations.\n");
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
            List<Variable> outputs = new List<Variable>();
            foreach (Variable v in combinations[0].GetLocals())
            {
                if (v.type == VariableType.OUTPUT)
                {
                    outputs.Add(v);
                }
            }

            return outputs;
        }

        public List<Variable> GetLocals()
        {
            return combinations[0].GetLocals();
        }
    }

    // A combination is a list of input variables and a list of the associated local variables for those inputs
    public class Combination
    {
        private List<Variable> inputs;
        private List<Variable> locals;

        public int NumInputs => inputs.Count;

        public int NumOutputs => locals.Count;

        public Combination()
        {
            
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

        public void SetCombo(List<Variable> inputs, List<Variable> locals)
        {
            this.inputs = inputs;
            this.locals = locals;
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

        public override bool Equals(object obj)
        {
            if (obj is Combination)
            {
                Combination otherCombo = (Combination) obj;

                if (otherCombo.locals.SequenceEqual(locals) && otherCombo.inputs.SequenceEqual(inputs))
                {
                    return true;
                }
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