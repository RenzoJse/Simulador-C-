using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface INamespaceService
{
    Namespace Create(CreateNamespaceArgs args);
}
