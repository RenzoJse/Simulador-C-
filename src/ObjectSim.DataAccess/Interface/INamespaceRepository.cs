using ObjectSim.Domain;

namespace ObjectSim.DataAccess.Interface;
public interface INamespaceRepository
{
    Namespace Add(Namespace ns);
    List<Namespace> GetAll();
    Namespace? GetByIdWithChildren(Guid id);
}
