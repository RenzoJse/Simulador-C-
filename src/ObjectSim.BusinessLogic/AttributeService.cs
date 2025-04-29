using ObjectSim.DataAccess.Interface;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
namespace ObjectSim.BusinessLogic;
public class AttributeService(IRepository<Attribute> attributeRepository) : IAttributeService
{
    public Attribute Create(Attribute attribute)
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
    public List<Attribute> GetAll()
    {
        var attributes = attributeRepository.GetAll(att1 => att1.Id != Guid.Empty);
        if(attributes == null || !attributes.Any())
        {
            throw new Exception("No attributes found.");
        }

        return attributes;
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
    public Attribute GetById(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id must be a valid non-empty GUID.", nameof(id));
        }

        var attribute = attributeRepository.Get(att => att.Id == id);
        if(attribute == null)
        {
            throw new KeyNotFoundException($"Attribute with ID {id} not found.");
        }

        return attribute;
    }

    public Attribute Update(Guid id, Attribute entity)
    {
        if(entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Attribute cannot be null.");
        }
        var existing = attributeRepository.Get(att => att.Id == entity.Id);
        if(existing == null)
        {
            throw new KeyNotFoundException($"Attribute with ID {entity.Id} not found.");
        }
        existing.Name = entity.Name;
        existing.DataType = entity.DataType;
        existing.ClassId = entity.ClassId;
        existing.Visibility = entity.Visibility;
        attributeRepository.Update(existing);
        return existing;
    }
}
