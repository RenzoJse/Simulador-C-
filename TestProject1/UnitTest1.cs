using ObjectSim;

namespace TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Sumar_DosNumerosPositivos_RetornaSuma()
    {
        var calc = new Test();
        var resultado = Test.Sumar(3, 5);
        Assert.AreEqual(9, resultado);
    }
}