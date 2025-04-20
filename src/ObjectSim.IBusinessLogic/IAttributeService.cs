namespace ObjectSim.IBusinessLogic;
public interface IAttributeService
{
    Domain.Attribute Create(Domain.Attribute attribute);
    Domain.Attribute GetById(Guid id); //TO-DO
}
