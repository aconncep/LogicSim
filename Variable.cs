namespace LogicSim
{
    public class Variable
    {
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

        public Variable(string name, int value)
        {
            Name = name;
            Value = value;
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
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}