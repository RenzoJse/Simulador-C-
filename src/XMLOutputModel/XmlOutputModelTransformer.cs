using ObjectSim.OutputModel;

namespace XMLOutputModel;

public class XmlOutputModelTransformer : IOutputModelTransformer
{
    public object Transform(object input)
    {
        if (input is string str)
        {
            var lines = str.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            var xml = lines.Aggregate("<ExecutionResult>", (current, line) => current + $"<Line>{System.Net.WebUtility.HtmlEncode(line)}</Line>");
            xml += "</ExecutionResult>";

            return new
            {
                format = "xml",
                content = xml
            };
        }

        return new
        {
            format = "plain",
            content = input?.ToString() ?? string.Empty
        };
    }
}
