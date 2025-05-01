using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
namespace ObjectSim.BusinessLogic;
public class AttributeService(IRepository<Attribute> attributeRepository, IClassService classService, IDataTypeService dataTypeService) : IAttributeService
{
    public Attribute CreateAttribute(CreateAttributeArgs args)
    {
        ValidateNullArgs(args);

        var visibility = ParseVisibility(args.Visibility);
        var dataType = dataTypeService.CreateDataType(args.DataType);

        var attribute = BuildAttribute(args, dataType, visibility);

        classService.AddAttribute(args.ClassId, attribute);
        AddAttributeToRepository(attribute);

        return attribute;
    }

    private static void ValidateNullArgs(CreateAttributeArgs args)
    {
        if(args == null)
        {
            throw new ArgumentNullException(nameof(args), "Attribute cannot be null.");
        }
    }

    private static Attribute.AttributeVisibility ParseVisibility(string visibilityValue)
    {
        if(!Enum.TryParse(visibilityValue, true, out Attribute.AttributeVisibility visibility))
        {
            throw new ArgumentException($"Invalid visibility value: {visibilityValue}");
        }
        return visibility;
    }

    private static Attribute BuildAttribute(CreateAttributeArgs args, DataType dataType, Attribute.AttributeVisibility visibility)
    {
        return new Attribute
        {
            Id = args.Id,
            Name = args.Name,
            DataType = dataType,
            ClassId = args.ClassId,
            Visibility = visibility
        };
    }

    private void AddAttributeToRepository(Attribute attribute)
    {
        attributeRepository.Add(attribute);
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
    public List<Attribute> GetByClassId(Guid classId)
    {
        if(classId == Guid.Empty)
        {
            throw new ArgumentException("ClassId must be a valid non-empty GUID.");
        }

        var attributes = attributeRepository.GetAll(a => a.ClassId == classId);
        if(!attributes.Any())
        {
            throw new KeyNotFoundException($"No attributes found for ClassId: {classId}");
        }

        return attributes.ToList();
    }
}
