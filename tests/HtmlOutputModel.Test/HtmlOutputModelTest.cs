using HtmlOutputModel;

namespace HtmlOutputModel2.Test;

[TestClass]
public class HtmlOutputModelTest
{
    [TestMethod]
    public void Transform_StringInput_ReturnsExpectedHtml()
    {
        var transformer = new HtmlOutputModelTransformer();
        const string input = "<b>Hola\nMundo</b>";
        var expectedHtml =
            "<section style='font-family: monospace; background: #f9f9f9; padding: 1em; border-radius: 6px;'>" +
            "<h3 style='color: #333;'>Resultado de la Ejecución</h3>" +
            "<ul style='list-style: none; padding-left: 0;'>" +
            "<li style='margin-bottom: 4px;'><code style='color: #007acc;'>&lt;b&gt;Hola</code></li>" +
            "<li style='margin-bottom: 4px;'><code style='color: #007acc;'>Mundo&lt;/b&gt;</code></li>" +
            "</ul></section>";

        var result = transformer.Transform(input);

        var format = result.GetType().GetProperty("format")?.GetValue(result, null);
        var content = result.GetType().GetProperty("content")?.GetValue(result, null);

        Assert.AreEqual("html", format);
        Assert.AreEqual(expectedHtml, content);
    }

    [TestMethod]
    public void Transform_NonStringInput_ReturnsPlainFormat()
    {
        var transformer = new HtmlOutputModelTransformer();
        const int input = 123;

        var result = transformer.Transform(input);

        var format = result.GetType().GetProperty("format")?.GetValue(result, null);
        var content = result.GetType().GetProperty("content")?.GetValue(result, null);

        Assert.AreEqual("plain", format);
        Assert.AreEqual("123", content);
    }
}
