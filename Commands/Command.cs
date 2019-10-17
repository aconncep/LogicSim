namespace LogicSim.Commands
{
    public abstract class Command
    {
        public virtual Variable[] Variables { get; } = new Variable[2];

        public abstract int ExpectedNumArguments();
        
        public void SetVariableValues(Variable[] vars)
        {
            int idx = 0;
            foreach (Variable var in vars)
            {
                Variables[idx] = var;
                idx++;
            }
        }

        public abstract int Evaluate();
        
        
    }
}