namespace ObjectSim.IBusinessLogic;

public interface IOutputModelTransformerService
{
    public void UploadDll(Stream dllStream, string fileName);
    public List<string> GetImplementationList();
    public void SelectImplementation(string name);
    public object TransformModel(string input);
}
