namespace ObjectSim.Security.Strategy;

public interface IValidationStrategy
{
    public bool WhichStrategy(string key);
    public bool Validate(string key);
}
