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

    #region GetAll

    public List<Attribute> GetAll()
    {
        var attributes = attributeRepository.GetAll(att1 => att1.Id != Guid.Empty);
        if(attributes == null || attributes.Count == 0)
        {
            throw new KeyNotFoundException("No attributes found.");
        }

        return attributes;
    }

    #endregion

    #region Delete

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

    #endregion

    #region GetById

    public Attribute GetById(Guid id)
    {
        ValidateGuidNotEmpty(id, nameof(id));
        return attributeRepository.Get(a => a.Id == id)
               ?? throw new KeyNotFoundException($"Attribute with ID {id} not found.");
    }

    private static void ValidateGuidNotEmpty(Guid id, string paramName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException($"{paramName} must be a valid non-empty GUID.", paramName);
        }
    }

    #endregion

    #region GetByClassId

    public List<Attribute> GetByClassId(Guid classId)
    {
        ValidateGuidNotEmpty(classId, nameof(classId));
        var attributes = attributeRepository.GetAll(a => a.ClassId == classId).ToList();
        if (attributes.Count == 0)
        {
            throw new KeyNotFoundException($"No attributes found for ClassId: {classId}");
        }

        return attributes;
    }

    #endregion

    #region Update

    public Attribute Update(Guid id, CreateAttributeArgs args)
    {
        ValidateAttributeArgsNotNull(args);

        var existing = attributeRepository.Get(a => a.Id == id)
                       ?? throw new KeyNotFoundException($"Attribute with ID {id} not found.");

        EnsureClassExists(args.ClassId);

        UpdateAttributeProperties(existing, args);

        attributeRepository.Update(existing);
        return existing;
    }

    private static void ValidateAttributeArgsNotNull(CreateAttributeArgs args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args), "Attribute arguments cannot be null.");
        }
    }

    private void EnsureClassExists(Guid classId)
    {
        if (classRepository.Get(c => c.Id == classId) == null)
        {
            throw new KeyNotFoundException($"Class with ID {classId} not found.");
        }
    }

    private void UpdateAttributeProperties(Attribute attribute, CreateAttributeArgs args)
    {
        attribute.Name = args.Name;
        attribute.ClassId = args.ClassId;
        attribute.Visibility = ParseVisibility(args.Visibility);
        attribute.DataType = dataTypeService.CreateDataType(args.DataType);
    }

    #endregion

}
