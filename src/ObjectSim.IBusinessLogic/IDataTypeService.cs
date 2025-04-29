using ObjectSim.Domain;
using Attribute = System.Attribute;

namespace ObjectSim.IBusinessLogic;

public interface IDataTypeService
{
    IDataType CreateDataType(Attribute dataType);
}
