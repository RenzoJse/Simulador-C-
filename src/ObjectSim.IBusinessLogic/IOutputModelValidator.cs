namespace ObjectSim.IBusinessLogic;

public interface IOutputModelValidator
{
    public List<string> GetImplementationList();
    public void SelectImplementation(string name);
    public bool ValidateModel(string modelValue);
}
