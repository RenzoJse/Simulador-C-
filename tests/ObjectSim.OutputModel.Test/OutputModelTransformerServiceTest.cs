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

        var outputModelService = new OutputModelTransformerService();

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

        var pluginsPath = Path.Combine(tempRoute, "Plugins");
        Directory.CreateDirectory(pluginsPath);

        TemporalAssembly.CreateTemporalAssembly(pluginsPath, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);

        outputModelService.SelectImplementation("TempType");

        var fieldInfo = typeof(OutputModelTransformerService).GetField("_validatorModel", BindingFlags.NonPublic | BindingFlags.Instance);
        var validatorModel = fieldInfo?.GetValue(outputModelService);

        validatorModel.Should().NotBeNull();
    }

    #endregion

    #region TransformModel

    [TestMethod]
    public void ValidateModel_WhenGivenAValidModel_ShouldReturnTrue()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var pluginsPath = Path.Combine(tempRoute, "Plugins");
        Directory.CreateDirectory(pluginsPath);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);

        outputModelService.SelectImplementation("TempType");

        const string model = "AAABBB";

        var result = outputModelService.TransformModel(model);
    }

    [TestMethod]
    public void ValidateModel_WhenNoModelIsSelected_ShouldThrowException()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var outputModelService = new OutputModelTransformerService(tempRoute);

        const string model = "AAABBB";

        var action = () => outputModelService.TransformModel(model);

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ValidateModel_WhenGivenAnInvalidModel_ShouldReturnFalse()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var pluginsPath = Path.Combine(tempRoute, "Plugins");
        Directory.CreateDirectory(pluginsPath);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var outputModelService = new OutputModelTransformerService(tempRoute);

        outputModelService.SelectImplementation("TempType");

        const string model = "N O T V A L I D";

        var result = outputModelService.TransformModel(model);
    }

    #endregion
}
