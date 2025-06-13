using System.Reflection;

namespace ObjectSim.OutputModel;

public class LoadAssemblers<TInterface>(string route)
    where TInterface : class
{
    private readonly DirectoryInfo _directory = new(Path.Combine(route, "Plugins"));
    private List<Type> _implementations = [];

    public List<string> GetImplementations()
    {
        var pluginsPath = Path.Combine(route, "Plugins");
        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
        }

        var files = _directory
            .GetFiles("*.dll")
            .ToList();

        _implementations = [];
        files.ForEach(file =>
        {
            var assemblyLoaded = Assembly.LoadFile(file.FullName);
            var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(TInterface).IsAssignableFrom(t))
                .ToList();

            if (loadedTypes.Count == 0)
            {
                Console.WriteLine(
                    $"There is no implementation of: {typeof(TInterface).Name} in the assembly: {file.FullName}");

                return;
            }

            _implementations = _implementations
                .Union(loadedTypes)
                .ToList();
        });

        return _implementations.ConvertAll(t => t.Name);
    }


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
