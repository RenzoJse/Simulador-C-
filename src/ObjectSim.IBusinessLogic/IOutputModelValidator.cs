namespace ObjectSim.IBusinessLogic;

public interface IOutputModelValidator
{
    public List<string> GetImplementationList();
    public void SelectImplementation(string name);
    public object Transform(object input);
}
