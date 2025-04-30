using ObjectSim.Domain.Args;
namespace ObjectSim.WebApi.DTOs.In;

public class CreateAttributeDtoIn
{
    public string Name { get; set; } = null!;
    public string DataTypeName { get; set; } = null!;
    public string DataTypeKind { get; set; } = null!;
    public string Visibility { get; set; } = null!;
    public Guid ClassId { get; set; }
    public CreateAttributeArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        ArgumentNullException.ThrowIfNull(Visibility);
        var dataTypeArgs = new CreateDataTypeArgs(DataTypeName, DataTypeKind);
        return new CreateAttributeArgs(dataTypeArgs, Visibility, ClassId, Name);
    }
}
