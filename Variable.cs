namespace LogicSim
{
    public enum VariableType
    {
        INPUT,LOCAL,OUTPUT
    }
    public class Variable
    {
        public VariableType type;
        public readonly string Name;
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (value == 0 || value == 1)
                {
                    _value = value;
                }
            }
        }

        public Variable(string name, int value, VariableType type)
        {
            Name = name;
            Value = value;
            this.type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is Variable)
            {
                Variable otherVar = (Variable) obj;
                return otherVar.Name == Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 100;
        }

        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }
}