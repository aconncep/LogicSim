using LogicSim.Commands;

namespace LogicSim
{
    /// <summary>
    /// Represents a single variable in the program
    /// </summary>
    public enum VarType
    {
        Input, Local
    }
    public class Variable
    {
        public VarType varType;
        public string name;
        public int value { get; private set; } = 0; // by default new variables are 0
        public bool shouldOutput;
        public bool isUsed = false;

        public Variable(VarType varType, string name, bool shouldOutput)
        {
            this.varType = varType;
            this.name = name;
            this.shouldOutput = shouldOutput;
        }

        /// <summary>
        /// Flip this variables bit 
        /// </summary>
        /// <returns></returns>
        public int FlipValue() 
        {
            if (value == 0)
            {
                value = 1;
                return 1;
            }
            else
            {
                value = 0;
                return 0;
            }
            
        }

        /// <summary>
        /// Manually sets the bit value of this variable
        /// </summary>
        /// <param name="newValue">new value for the bit, 0 or 1</param>
        public void SetValue(int newValue)
        {
            if (newValue == 0 || newValue == 1)
            {
                value = newValue;
            }

        }

        /// <summary>
        /// String representation of the variable, used for debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (varType == VarType.Local)
            {
                return "Local Variable " + name + " with value " + value;
            }
            return "Input Variable " + name + " with value " + value;  
        }

        /// <summary>
        /// Two variable objects represent the same variable if they share the same name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Variable)
            {
                Variable otherVar = (Variable)obj;
                return name == otherVar.name;
            }
            return false;
        }
    }
}
