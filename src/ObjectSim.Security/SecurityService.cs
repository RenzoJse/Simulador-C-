using ObjectSim.Security.Strategy;

namespace ObjectSim.Security;

public class SecurityService(IEnumerable<IValidationStrategy> securityStrategies) : ISecurityService
{

    public bool IsValidKey(string key)
    {
        return SelectSecurityStrategy(key);
    }

    private bool SelectSecurityStrategy(string text)
    {
        var strategy = securityStrategies.FirstOrDefault(s => s.WhichStrategy(text));
        if(strategy == null)
        {
            throw new ArgumentException("No security strategy found for the given arguments.");
        }

        return strategy.Validate(text);
    }

}
