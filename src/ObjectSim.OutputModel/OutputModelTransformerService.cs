using ObjectSim.Abstractions;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.OutputModel;

public class OutputModelTransformerService()
    : IOutputModelTransformerService
{
    private readonly string _route = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);

    private IOutputModelTransformer? _transformerModel;

    public OutputModelTransformerService(string route)
        : this()
    {
        _route = route;
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
        if (_transformerModel == null)
        {
            throw new InvalidOperationException("No transformer selected.");
        }

        var model = new Model(input);
        return _transformerModel.Transform(model);
    }
}
