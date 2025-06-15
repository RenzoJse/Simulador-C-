using ObjectSim.Domain;

namespace ObjectSim.Examples;
public class ExampleService
{
    private List<Class> _visitorExampleClasses = [];

    public void LoadVisitorExample()
    {
        _visitorExampleClasses = VisitorExampleFactory.CreateVisitorExample();
    }

    public List<Class> GetExampleClasses()
    {
        return _visitorExampleClasses;
    }
}
