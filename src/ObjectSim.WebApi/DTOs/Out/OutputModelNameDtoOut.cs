namespace ObjectSim.WebApi.DTOs.Out;

public record OutputModelNameDtoOut
{
    public string Name { get; set; } = String.Empty;

    public static OutputModelNameDtoOut ToInfo(string name)
    {
        return new OutputModelNameDtoOut
        {
            Name = name
        };
    }
}
