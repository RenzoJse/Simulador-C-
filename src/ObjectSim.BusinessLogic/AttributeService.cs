using ObjectSim.DataAccess.Interface;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class AttributeService(IRepository<Domain.Attribute> attributeRepository) : IAttributeService<Domain.Attribute>
{
    public Domain.Attribute Create(Domain.Attribute attribute)
    {
        if(attribute == null)
        {
            throw new InvalidOperationException("Attribute cannot be null.");
        }
        else
        {
            attributeRepository.Add(attribute!);
            return attribute;
        }
    }

    public List<Domain.Attribute> GetAll()
    {
        var attributes = attributeRepository.GetAll(att1 => att1.Id != Guid.Empty);
        if(attributes == null || !attributes.Any())
        {
            throw new Exception("No attributes found.");
        }

        return (List<Domain.Attribute>)attributes;
    }
    public bool Delete(Guid id)
    {
        var attribute = attributeRepository.Get(att1 => id == att1.Id);
        if(attribute == null)   
        {
            throw new Exception("Attribute cannot be null.");
        }
        attributeRepository.Delete(attribute);
        return true;
    }


}
