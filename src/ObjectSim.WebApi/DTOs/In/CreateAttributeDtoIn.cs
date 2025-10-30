using System.ComponentModel.DataAnnotations;
using ObjectSim.Domain.Args;
namespace ObjectSim.WebApi.DTOs.In;

public record CreateAttributeDtoIn
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string DataTypeId { get; set; } = null!;
    [Required]
    public string Visibility { get; set; } = null!;
    [Required]
    public Guid ClassId { get; set; }
    [Required]
    public bool IsStatic { get; set; }
    public CreateAttributeArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        ArgumentNullException.ThrowIfNull(Visibility);
        return new CreateAttributeArgs(Guid.Parse(DataTypeId), Visibility, ClassId, Name, IsStatic);
    }
}
