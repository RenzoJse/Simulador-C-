namespace ObjectSim.OutputModel.Test;

[TestClass]
public class LoadAssemblersTest
{
    #region GetImplementations

    [TestMethod]
    public void GetImplementation_WhenInvalidName_ThrowsException()
    {
        var temporalRoute = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temporalRoute);

        TemporalAssembly.CreateTemporalAssembly(temporalRoute, "TempAssembly");

        var loadAssemblers = new LoadAssemblers<IModeloValidador>(temporalRoute);

        var act = () => loadAssemblers.GetImplementations("InvalidImplementation");

        act.Should().Throw<InvalidOperationException>().WithMessage("No implementation found: InvalidImplementation");
    }

    #endregion
}
