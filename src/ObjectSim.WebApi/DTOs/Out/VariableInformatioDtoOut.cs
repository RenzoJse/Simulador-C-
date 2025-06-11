using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record VariableInformatioDtoOut
{
    public string Name { get; set; } = String.Empty;
    public Guid TypeId { get; init; }
    public Guid VariableId { get; init; }

    public static VariableInformatioDtoOut ToInfo(Variable variable)
    {
        return new VariableInformatioDtoOut
        {
            Name = variable.Name,
            TypeId = variable.TypeId,
            VariableId = variable.VariableId
        };
    }
}
