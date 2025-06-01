namespace ObjectSim.Security.Test.Strategy.Key;

[TestClass]
public class KeyTest
{
    #region CreateKey

    #region Success

    [TestMethod]
    public void CreateKey_ValidInput_CreatesKey()
    {
        var key = new Security.Strategy.KeyStrat.Key();
        Assert.IsNotNull(key);
        Assert.AreNotEqual(Guid.Empty, key.AccessKey);
    }

    #endregion

    #endregion
}
