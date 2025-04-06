using ObjectSim;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Sumar_DosNumerosPositivos_RetornaSuma()
    {
        var resultado = Test.Sumar(3, 6);
        Assert.AreEqual(10, resultado);
    }
}
