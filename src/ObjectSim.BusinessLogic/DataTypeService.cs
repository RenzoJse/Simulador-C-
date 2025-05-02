using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;

public class DataTypeService(IRepository<Class> classRepository) : IDataTypeService
{
    public DataType CreateDataType(CreateDataTypeArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        return args.Kind.ToLower() switch
        {
            "value" => new Domain.ValueType(args.Name, "Value", []),
            "reference" => CreateReferenceType(args.Name),
            _ => throw new ArgumentException($"Invalid kind: {args.Kind}")
        };
    }
    private ReferenceType CreateReferenceType(string name)
    {
        if(name == "object")
        {
            var classExists = classRepository.GetAll(c => c.Name == name).Any();
            if(!classExists)
            {
                throw new ArgumentException("Cannot create ReferenceType of 'object' if no class with that name exists.");
            }
        }

        return new ReferenceType(name, "Reference", []);
    }
}
