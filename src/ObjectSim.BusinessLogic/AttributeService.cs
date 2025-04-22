using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class AttributeService(IAttributeRepository attributeRepository) : IAttributeService
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
    public Domain.Attribute GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
