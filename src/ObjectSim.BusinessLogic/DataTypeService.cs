using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;

public class DataTypeService(IRepository<Class> classRepository) : IDataTypeService
{
    public DataType CreateDataType(CreateDataTypeArgs dataType)
    {
        throw new NotImplementedException();
    }
}
