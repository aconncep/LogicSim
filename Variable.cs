namespace LogicSim
{
    public class Variable
    {
        public string name;
        public int value;

        public Variable(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Variable)
            {
                Variable otherVar = (Variable) obj;
                return otherVar.name == this.name;
            }

            return false;
        }

        public override string ToString()
        {
            return name;
        }
    }
}