namespace ObjectSim.BusinessLogic.ClassesBuilders.Builders;

public class ClassBuilder : Builder
{
    public override void SetAttributes(List<Attribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
    }
}
