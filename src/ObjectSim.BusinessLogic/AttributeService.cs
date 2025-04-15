using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class AttributeService(IAttributeRepository attributeRepository) : IAttributeService
{
    public Domain.Attribute Create(Domain.Attribute attribute) // Explicitly specify Domain.Attribute
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
}
