using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateVariableDtoIn
{
    public string Name { get; set; } = String.Empty;
    public string TypeId { get; set; } = String.Empty;

    public CreateVariableArgs ToArgs()
    {
        return new CreateVariableArgs(Guid.Parse(TypeId), Name);
    }
}
