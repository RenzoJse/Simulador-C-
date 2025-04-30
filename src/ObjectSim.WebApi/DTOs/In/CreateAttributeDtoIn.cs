using System.ComponentModel.DataAnnotations;
using ObjectSim.Domain.Args;
namespace ObjectSim.WebApi.DTOs.In;

public class CreateAttributeDtoIn
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string DataTypeName { get; set; } = null!;
    [Required]
    public string DataTypeKind { get; set; } = null!;
    [Required]
    public string Visibility { get; set; } = null!;
    [Required]
    public Guid ClassId { get; set; }
    public CreateAttributeArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        ArgumentNullException.ThrowIfNull(Visibility);
        var dataTypeArgs = new CreateDataTypeArgs(DataTypeName, DataTypeKind);
        return new CreateAttributeArgs(dataTypeArgs, Visibility, ClassId, Name);
    }
}
