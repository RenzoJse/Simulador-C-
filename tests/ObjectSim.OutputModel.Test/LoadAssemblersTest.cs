namespace ObjectSim.OutputModel.Test;

[TestClass]
public class LoadAssemblersTest
{
    #region GetImplementations

    #region Success

    [TestMethod]
    public void GetImplementations_WhenGivingRouteWithoutAssemblers_ReturnsEmptyList()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        var loadAssemblers = new LoadAssemblers<IOutputModel>(tempRoute);

        var implementations = loadAssemblers.GetImplementations();

        implementations.Should().BeEmpty();

        Directory.Delete(tempRoute, true);
    }

    [TestMethod]
    public void GetImplementations_WhenGivingRouteWithAssemblers_ReturnsList()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        TemporalAssembly.CreateTemporalAssembly(tempRoute, "TempAssembly");

        var loadAssemblers = new LoadAssemblers<IOutputModel>(tempRoute);

        var implementations = loadAssemblers.GetImplementations();

        implementations.Should().NotBeEmpty();
        implementations.Count.Should().Be(1);
        implementations.Should().Contain("TempType");
    }

    [TestMethod]
    public void GetImplementations_WhenGivingRouteWithAssemblersWithoutImplementations_ReturnsEmptyList()
    {
        var tempRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempRoute);

        const string code = """
                              public class TempType
                              {
                              }
                              """;

        var compilation = TemporalAssembly.CreateCompilation(code, "TempAssembly");
        var assemblyPath = Path.Combine(tempRoute, "TempAssembly.dll");
        TemporalAssembly.SaveCompilationOnDisc(compilation, assemblyPath);

        var loadAssemblers = new LoadAssemblers<IOutputModel>(tempRoute);

        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        var implementations = loadAssemblers.GetImplementations();

        implementations.Should().BeEmpty();
        consoleOutput.ToString().Should().Contain($"No one implements interface of: {nameof(IOutputModel)} in the assembly: {assemblyPath}");
    }

    #endregion

    #endregion

    #region GetImplementation

    #region Error

    [TestMethod]
    public void GetImplementation_WhenInvalidName_ThrowsException()
    {
        var temporalRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temporalRoute);

        TemporalAssembly.CreateTemporalAssembly(temporalRoute, "TempAssembly");

        var loadAssemblers = new LoadAssemblers<IOutputModel>(temporalRoute);

        var act = () => loadAssemblers.GetImplementation("InvalidImplementation");

        act.Should().Throw<InvalidOperationException>().WithMessage("No implementation found: InvalidImplementation");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetImplementation_WhenInstanceValidName_ReturnsImplementationInstance()
    {
        var temporalRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temporalRoute);

        TemporalAssembly.CreateTemporalAssembly(temporalRoute, "TempAssembly");

        var loadAssemblers = new LoadAssemblers<IOutputModel>(temporalRoute);

        loadAssemblers.GetImplementations();

        var implementation = loadAssemblers.GetImplementation("TempType");

        implementation.Should().NotBeNull();
    }

    #endregion

    #endregion
}
