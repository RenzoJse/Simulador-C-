using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IDataTypeService
{
    DataType CreateDataType(CreateDataTypeArgs dataType);
    DataType GetById(Guid id);
}
