using ObjectSim.IBusinessLogic;

namespace ObjectSim.OutputModel;

public class OutputModelTransformerService()
    : IOutputModelTransformerService
{
    private readonly string _route = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

    private IOutputModelTransformer? _transformerModel;

    public OutputModelTransformerService(string route)
        : this()
    {
        _route = route;
    }

    public void UploadDll(Stream dllStream, string fileName)
    {
        if(dllStream == null || string.IsNullOrWhiteSpace(fileName) || !fileName.EndsWith(".dll"))
        {
            throw new ArgumentException("Invalid File Type.");
        }

        Directory.CreateDirectory(_route);
        var savePath = Path.Combine(_route, fileName);

        if(File.Exists(savePath))
        {
            throw new InvalidOperationException("El archivo ya existe.");
        }

        using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        dllStream.CopyTo(fileStream);
    }

    public List<string> GetImplementationList()
    {
        var loadAssemblers = new LoadAssemblers<IOutputModelTransformer>(_route);

        return loadAssemblers.GetImplementations();
    }

    public void SelectImplementation(string name)
    {
        var loadAssemblers = new LoadAssemblers<IOutputModelTransformer>(_route);

        loadAssemblers.GetImplementations();

        _transformerModel = loadAssemblers.GetImplementation(name);
    }

    public object TransformModel(string input)
    {
        if(_transformerModel == null)
        {
            throw new InvalidOperationException("No transformer selected.");
        }

        return _transformerModel.Transform(input);
    }
}
