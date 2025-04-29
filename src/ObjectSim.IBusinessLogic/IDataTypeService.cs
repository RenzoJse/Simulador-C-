using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IDataTypeService
{
    IDataType CreateDataType(CreateDataTypeArgs dataType);
}
