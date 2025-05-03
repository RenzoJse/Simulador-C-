using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.IBusinessLogic;

public interface IClassServiceBuilder
{
    public void AddAttribute(Guid? classId, Attribute attribute);

}
