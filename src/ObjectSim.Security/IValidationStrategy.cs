namespace ObjectSim.Security;

public interface IValidationStrategy
{
    public bool Validate(string key);
}
