using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;

namespace ObjectSim.Security.Strategy.KeyStrat;

public class KeyStrategy(IRepository<Key> keyRepository) : IValidationStrategy
{

    public bool WhichStrategy(string key)
    {
        return Guid.TryParse(key, out _);
    }

    public bool Validate(string key)
    {
        return keyRepository.Exists(k => k.AccessKey.ToString() == key);
    }
}
