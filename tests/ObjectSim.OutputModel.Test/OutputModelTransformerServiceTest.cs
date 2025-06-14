using System.Reflection;
using FluentAssertions;

namespace ObjectSim.OutputModel.Test;

[TestClass]
public class OutputModelTransformerServiceTest
{
    #region ListImplementations

    [TestMethod]
    public void GetImplementations_WhenRouteWithoutAssemblers_ShouldReturnEmptyList()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var outputModelService = new OutputModelTransformerService(tempRoute);

        var implementations = outputModelService.GetImplementationList();

        implementations.Should().BeEmpty();
    }

    [TestMethod]
    public void GetImplementations_WhenRouteWithAssemblers_ShouldReturnList()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);

        var implementations = outputModelService.GetImplementationList();

        implementations.Should().NotBeEmpty();
        implementations.Count.Should().Be(1);
        implementations.Should().Contain("TempType");
    }

    #endregion

    #region SeleccionarImplementacion

    #region Error

    [TestMethod]
    public void SelectImplementation_WhenGivenAnOutOfRangeIndex_ShouldThrowException()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var pluginsPath = Path.Combine(tempRoute, "Plugins");
        Directory.CreateDirectory(pluginsPath);

        var outputModelService = new OutputModelTransformerService(tempRoute);

        var action = () => outputModelService.SelectImplementation("InvalidName");

        action.Should().Throw<InvalidOperationException>();
    }

    #endregion

    [TestMethod]
    public void SelectImplementation_WhenGivenAValidIndex_ShouldSaveTheModel()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);

        outputModelService.SelectImplementation("TempType");

        var fieldInfo = typeof(OutputModelTransformerService).GetField("_transformerModel", BindingFlags.NonPublic | BindingFlags.Instance);
        var validatorModel = fieldInfo?.GetValue(outputModelService);

        validatorModel.Should().NotBeNull();
    }

    #endregion

    #region TransformModel

    [TestMethod]
    public void TransformModel_WhenGivenAValidModel_ShouldReturnTrue()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);
        outputModelService.SelectImplementation("TempType");

        const string input = "TestString";

        var result = outputModelService.TransformModel(input);

        result.Should().Be(input);
    }

    [TestMethod]
    public void TransformModel_WhenNoTransformerSelected_ShouldThrowException()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var outputModelService = new OutputModelTransformerService(tempRoute);

        Action act = () => outputModelService.TransformModel("TestString");

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("No transformer selected.");
    }

    #endregion

    #region UploadDll

    [TestMethod]
    public void UploadDll_ValidDll_SavesFile()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);
        var service = new OutputModelTransformerService(tempRoute);

        const string fileName = "test.dll";
        var content = new byte[] { 1, 2, 3 };
        using var stream = new MemoryStream(content);

        service.UploadDll(stream, fileName);

        var savedPath = Path.Combine(tempRoute, fileName);
        File.Exists(savedPath).Should().BeTrue();
        File.ReadAllBytes(savedPath).Should().BeEquivalentTo(content);
    }

    [TestMethod]
    public void UploadDll_NullStream_ThrowsArgumentException()
    {
        var service = new OutputModelTransformerService("anyPath");
        Action act = () => service.UploadDll(null!, "test.dll");
        act.Should().Throw<ArgumentException>().WithMessage("Invalid File Type.");
    }

    [TestMethod]
    public void UploadDll_EmptyFileName_ThrowsArgumentException()
    {
        var service = new OutputModelTransformerService("anyPath");
        using var stream = new MemoryStream();
        Action act = () => service.UploadDll(stream, "");
        act.Should().Throw<ArgumentException>().WithMessage("Invalid File Type.");
    }

    [TestMethod]
    public void UploadDll_NotDllExtension_ThrowsArgumentException()
    {
        var service = new OutputModelTransformerService("anyPath");
        using var stream = new MemoryStream();
        Action act = () => service.UploadDll(stream, "test.txt");
        act.Should().Throw<ArgumentException>().WithMessage("Invalid File Type.");
    }

    #endregion
}
