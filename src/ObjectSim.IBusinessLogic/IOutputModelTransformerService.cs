namespace ObjectSim.IBusinessLogic;

public interface IOutputModelTransformerService
{
    public List<string> GetImplementationList();
    public void SelectImplementation(string name);
    public object TransformModel(string input);
}
