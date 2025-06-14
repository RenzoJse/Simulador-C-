using XMLOutputModel;

namespace XmlOutputModel.Test;

[TestClass]
public class XmlOutputModelTransformerTest
{
    [TestMethod]
    public void Transform_StringInput_ReturnsExpectedXml()
    {
        var transformer = new XmlOutputModelTransformer();
        const string input = "Linea1\nLinea2\r\nLinea3";
        const string expectedXml = "<ExecutionResult>" +
                                   "<Line>Linea1</Line>" +
                                   "<Line>Linea2</Line>" +
                                   "<Line>Linea3</Line>" +
                                   "</ExecutionResult>";

        var result = transformer.Transform(input);

        Assert.IsNotNull(result);
        var format = result.GetType().GetProperty("format")?.GetValue(result, null);
        var content = result.GetType().GetProperty("content")?.GetValue(result, null);

        Assert.AreEqual("xml", format);
        Assert.AreEqual(expectedXml, content);
    }

    [TestMethod]
    public void Transform_NonStringInput_ReturnsPlainFormat()
    {
        var transformer = new XmlOutputModelTransformer();
        const int input = 123;

        var result = transformer.Transform(input);

        Assert.IsNotNull(result);
        var format = result.GetType().GetProperty("format")?.GetValue(result, null);
        var content = result.GetType().GetProperty("content")?.GetValue(result, null);

        Assert.AreEqual("plain", format);
        Assert.AreEqual("123", content);
    }

    [TestMethod]
    public void Transform_NullInput_ReturnsPlainEmptyString()
    {
        var transformer = new XmlOutputModelTransformer();

        var result = transformer.Transform(null!);

        Assert.IsNotNull(result);
        var format = result.GetType().GetProperty("format")?.GetValue(result, null);
        var content = result.GetType().GetProperty("content")?.GetValue(result, null);

        Assert.AreEqual("plain", format);
        Assert.AreEqual(string.Empty, content);
    }
}
