using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
namespace ObjectSim.BusinessLogic;
public class AttributeService(IRepository<Attribute> attributeRepository, IRepository<Class> classRepository, IDataTypeService dataTypeService) : IAttributeService
{

    #region CreateAttribute

    public Attribute CreateAttribute(CreateAttributeArgs args)
    {
        var attribute = BuildAttributeFromArgs(args);

        AddAttributeToClass(args.ClassId, attribute);

        SaveAttribute(attribute);

        return attribute;
    }

    private Attribute BuildAttributeFromArgs(CreateAttributeArgs args)
    {
        var visibility = ParseVisibility(args.Visibility);
        var dataType = dataTypeService.CreateDataType(args.DataType);

        return new Attribute
        {
            Id = args.Id,
            Name = args.Name,
            DataType = dataType,
            ClassId = args.ClassId,
            Visibility = visibility
        };
    }

    private void AddAttributeToClass(Guid? classId, Attribute attribute)
    {
        if (classId is null)
        {
            throw new ArgumentNullException(nameof(classId));
        }

        var classObj = GetClassById(classId.Value);

        classObj.AddAttribute(attribute);
    }

    private void SaveAttribute(Attribute attribute)
    {
        attributeRepository.Add(attribute);
    }

    private Class GetClassById(Guid classId)
    {
        return classRepository.Get(c => c.Id == classId)
               ?? throw new ArgumentException("Class not found.");
    }

    private static Attribute.AttributeVisibility ParseVisibility(string visibilityValue)
    {
        if (!Enum.TryParse(visibilityValue, true, out Attribute.AttributeVisibility visibility))
        {
            throw new ArgumentException($"Invalid visibility value: {visibilityValue}");
        }

        return visibility;
    }

    #endregion

    public List<Attribute> GetAll()
    {
        var attributes = attributeRepository.GetAll(att1 => att1.Id != Guid.Empty);
        if(attributes == null || !attributes.Any())
        {
            throw new KeyNotFoundException("No attributes found.");
        }

        return attributes;
    }

    public bool Delete(Guid id)
    {
        var attribute = attributeRepository.Get(att1 => att1.Id == id);
        if(attribute == null)
        {
            throw new KeyNotFoundException($"Attribute with ID {id} not found.");
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

    public Attribute Update(Guid id, CreateAttributeArgs entity)
    {
        if(entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Attribute arguments cannot be null.");
        }

        var existing = attributeRepository.Get(att => att.Id == id);
        if(existing == null)
        {
            throw new KeyNotFoundException($"Attribute with ID {id} not found.");
        }
        var classExists = classRepository.Get(c => c.Id == entity.ClassId);
        if(classExists == null)
        {
            throw new KeyNotFoundException($"Class with ID {entity.ClassId} not found.");
        }

        var visibility = ParseVisibility(entity.Visibility);
        var dataType = dataTypeService.CreateDataType(entity.DataType);

        existing.Name = entity.Name;
        existing.ClassId = entity.ClassId;
        existing.Visibility = visibility;
        existing.DataType = dataType;

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
