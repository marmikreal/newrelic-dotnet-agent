namespace NewRelic.Agent.Core.Configuration
{
    public class SecurityPolicy
    {
        public string Name { get; private set; }

        public bool Enabled { get; private set; }

        public SecurityPolicy(string name, bool enabled)
        {
            Name = name;
            Enabled = enabled;
        }
    }
}
