using ObjectSim;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Sumar_DosNumerosPositivos_RetornaSuma()
    {
        var resultado = Test.Sumar(4, 7);
        Assert.AreEqual(12, resultado);
    }
}
