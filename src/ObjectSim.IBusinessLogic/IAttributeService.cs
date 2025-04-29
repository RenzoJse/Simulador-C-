namespace ObjectSim.IBusinessLogic;
public interface IAttributeService
{
    Domain.Attribute CreateAttribute(Domain.Attribute attribute);
    List<Domain.Attribute> GetAll();
    bool Delete(Guid id);
    Domain.Attribute GetById(Guid id);
    Domain.Attribute Update(Guid id, Domain.Attribute entity);
}
