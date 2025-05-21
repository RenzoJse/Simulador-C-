using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;

public class DataTypeService(IRepository<Class> classRepository, IRepository<DataType> dataTypeRepository) : IDataTypeService
{

    #region CreateDataType

    public DataType CreateDataType(CreateDataTypeArgs args)
    {
        ValidateArgsNotNull(args);

        if(IsBuiltinType(args.Type))
        {
            return CreateBuiltinValueType(args);
        }

        if(IsStringType(args.Type))
        {
            return CreateStringReferenceType(args);
        }

        if(!ClassExists(args.Type))
        {
            throw new ArgumentException($"Cannot create ReferenceType: class '{args.Type}' not found.");
        }

        return CreateReferenceType(args);
    }

    private static void ValidateArgsNotNull(CreateDataTypeArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
    }

    private static bool IsBuiltinType(string type)
    {
        return Domain.ValueType.BuiltinTypes.Contains(type);
    }

    private static bool IsStringType(string type)
    {
        return type == "string";
    }

    private bool ClassExists(string typeName)
    {
        return classRepository.GetAll(c => c.Name == typeName).Count != 0;
    }

    private static Domain.ValueType CreateBuiltinValueType(CreateDataTypeArgs args)
    {
        return new Domain.ValueType(args.Name, args.Type, []);
    }

    private static ReferenceType CreateStringReferenceType(CreateDataTypeArgs args)
    {
        return new ReferenceType(args.Name, args.Type, []);
    }

    private static ReferenceType CreateReferenceType(CreateDataTypeArgs args)
    {
        return new ReferenceType(args.Name, args.Type, []);
    }
    public DataType GetById(Guid id)
    {
        var dt = dataTypeRepository.Get(c => c.Id == id);
        return dt is null ? throw new KeyNotFoundException("DataType not found") : dt;
    }
    public List<DataType> GetAll()
    {
        return dataTypeRepository.GetAll(_ => true);
    }
    #endregion

}
