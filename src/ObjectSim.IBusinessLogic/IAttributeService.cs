using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface IAttributeService
{
    Domain.Attribute CreateAttribute(CreateAttributeArgs attribute);
    List<Domain.Attribute> GetAll();
    bool Delete(Guid id);
    Domain.Attribute GetById(Guid id);
    Domain.Attribute Update(Guid id, Domain.Attribute entity);
    Domain.Attribute GetByClassId(Guid classId);
}
