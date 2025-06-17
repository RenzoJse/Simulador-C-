using ObjectSim.OutputModel;

namespace HtmlOutputModel;

public class HtmlOutputModelTransformer : IOutputModelTransformer
{
    public object Transform(object input)
    {
        if(input is string str)
        {
            var lines = str.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            var html = "<section style='font-family: monospace; background: #f9f9f9; padding: 1em; border-radius: 6px;'>";
            html += "<h3 style='color: #333;'>Resultado de la Ejecución</h3>";
            html += "<ul style='list-style: none; padding-left: 0;'>";
            html = lines.Aggregate(html, (current, line) =>
                current + $"<li style='margin-bottom: 4px;'><code style='color: #007acc;'>{System.Net.WebUtility.HtmlEncode(line)}</code></li>");
            html += "</ul></section>";

            return new
            {
                format = "html",
                content = html
            };
        }

        return new
        {
            format = "plain",
            content = input?.ToString() ?? string.Empty
        };
    }
}
