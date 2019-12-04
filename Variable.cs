namespace LogicSim
{
    public enum VariableType
    {
        INPUT,LOCAL,OUTPUT
    }
    public class Variable
    {
        public readonly VariableType type;
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
                return otherVar.Name == Name && otherVar._value == _value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }
}