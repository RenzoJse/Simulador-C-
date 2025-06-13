using System.Reflection;

namespace ObjectSim.OutputModel;

public class LoadAssemblers<TInterface>()
    where TInterface : class
{
    private List<Type> _implementations = [];

    public TInterface GetImplementation(string name, params object[] args)
    {
        var indice = _implementations.FindIndex(t => t.Name == name);
        var type = _implementations.ElementAtOrDefault(indice);

        if (type == null)
        {
            throw new InvalidOperationException($"No implementation found: {name}");
        }

        return (Activator.CreateInstance(type, args) as TInterface)!;
    }
}
