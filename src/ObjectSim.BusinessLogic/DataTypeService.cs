using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;

public class DataTypeService(IRepository<Class> classRepository) : IDataTypeService
{
    public DataType CreateDataType(CreateDataTypeArgs args)
    {//si esta en la lista de clases, traer todos los metodos pubicos y asignarlos a la lista.
     //precargar los metodos de .NET que esos se levantan automaticamente de la bd.CargarMetodosDeEnteros y demas. mirar link que pasa profe
        ArgumentNullException.ThrowIfNull(args);

        if(Domain.ValueType.BuiltinTypes.Contains(args.Type))
        {
            return new Domain.ValueType(args.Name, args.Type, []);
        }
        if(args.Type == "string")
        {
            return new ReferenceType(args.Name, args.Type, []);
        }
        var classExists = classRepository.GetAll(c => c.Name == args.Type).Any();
        if(!classExists)
        {
            throw new ArgumentException($"Cannot create ReferenceType: class '{args.Type}' not found.");
        }
        return new ReferenceType(args.Name, args.Type, []);
    }
}
