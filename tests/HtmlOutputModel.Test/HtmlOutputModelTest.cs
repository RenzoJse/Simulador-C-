using HtmlOutputModel;

namespace HtmlOutputModel2.Test;

[TestClass]
public class HtmlOutputModelTest
{
    [TestMethod]
    public void Transform_StringInput_ReturnsHtmlEncodedPre()
    {
        var transformer = new HtmlOutputModelTransformer("test");
        const string input = "<b>Hola\nMundo</b>";

        var result = transformer.Transform(input);

        Assert.AreEqual("<pre>&lt;b&gt;Hola\nMundo&lt;/b&gt;</pre>", result);
    }

    [TestMethod]
    public void Transform_NonStringInput_ReturnsInput()
    {
        var transformer = new HtmlOutputModelTransformer("test");
        const int input = 123;

        var result = transformer.Transform(input);

        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void Name_Property_ReturnsConstructorValue()
    {
        var transformer = new HtmlOutputModelTransformer("myName");

        Assert.AreEqual("myName", transformer.Name);
    }
}
