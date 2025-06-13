using ObjectSim.Abstractions;

namespace HtmlOutputModel;

public class HtmlOutputModelTransformer(string? name) : IOutputModelTransformer
{
    public object Transform(object input)
    {
        if (input is string str)
        {
            return $"<pre>{System.Net.WebUtility.HtmlEncode(str)}</pre>";
        }
        return input;
    }

    public string Name { get; } = name!;
}
