using ObjectSim.Abstractions;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.OutputModel;

public class OutputModelValidator() : IOutputModelValidator
{
    private readonly string _route = AppDomain.CurrentDomain.BaseDirectory;

    private IOutputModelValidator? _validatorModel;

    public OutputModelValidator(string route)
        : this()
    {
        _route = route;
    }

    public List<string> GetImplementationList()
    {
        var loadAssemblers = new LoadAssemblers<IOutputModelValidator>(_route);

        return loadAssemblers.GetImplementations();
    }

    public void SelectImplementation(string name)
    {
        var loadAssemblers = new LoadAssemblers<IOutputModelValidator>(_route);

        loadAssemblers.GetImplementations();

        _validatorModel = loadAssemblers.GetImplementation(name);
    }

    public bool ValidateModel(string modelValue)
    {
        if (_validatorModel == null)
        {
            throw new InvalidOperationException("No validation logic has been selected.");
        }

        var model = new Model(modelValue);
        return _validatorModel.ValidateModel(model.Value);
    }
}
