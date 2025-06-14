using ObjectSim.OutputModel;

namespace HtmlOutputModel;

public class HtmlOutputModelTransformer : IOutputModelTransformer
{
    public object Transform(object input)
    {
        if (input is string str)
        {
            var lines = str.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            var html = "<section>";
            html += "<h3>Resultado de la Ejecución</h3>";
            html += "<ul>";
            html = lines.Aggregate(html, (current, line) => current + $"<li><code>{System.Net.WebUtility.HtmlEncode(line)}</code></li>");
            html += "</ul>";
            html += "</section>";
            return html;
        }
        return input;
    }
}
