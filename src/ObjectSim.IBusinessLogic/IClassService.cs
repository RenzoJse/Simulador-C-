using ObjectSim.Domain;

namespace ObjectSim.IBusinessLogic;

public interface IClassService
{
    public Class GetById(Guid id);
}
