using System.Linq.Expressions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Security.Strategy.KeyStrat;
using Keytest = ObjectSim.Security.Strategy.KeyStrat.Key;

namespace ObjectSim.Security.Test.Strategy.Key;

[TestClass]
public class KeyStrategyTest
{
    private Mock<IRepository<Keytest>>? _keyRepositoryMock;
    private KeyStrategy? _keyStrategy;

    [TestInitialize]
    public void Initialize()
    {
        _keyRepositoryMock = new Mock<IRepository<Keytest>>(MockBehavior.Strict);
        _keyStrategy = new KeyStrategy(_keyRepositoryMock.Object);
    }

    #region WhichStrategy

    #region Error

    [TestMethod]
    public void WhichStrategy_ReturnsFalse_WhenKeyIsNotGuid()
    {
        const string invalidKey = "not-a-guid";
        var result = _keyStrategy!.WhichStrategy(invalidKey);
        Assert.IsFalse(result);
    }

    #endregion

    #region Success

    [TestMethod]
    public void WhichStrategy_ReturnsTrue_WhenKeyIsValidGuid()
    {
        var validGuid = Guid.NewGuid().ToString();
        var result = _keyStrategy!.WhichStrategy(validGuid);
        Assert.IsTrue(result);
    }

    #endregion

    #endregion

    #region Validate

    #region Error

    [TestMethod]
    public void Validate_ReturnsFalse_WhenRepositoryDoesNotFindKey()
    {
        var keyString = Guid.NewGuid().ToString();
        _keyRepositoryMock!.Setup(r => r.Exists(It.IsAny<Expression<Func<Keytest, bool>>>()))
            .Returns(false);

        var result = _keyStrategy!.Validate(keyString);
        Assert.IsFalse(result);
    }

    #endregion

    #region Success

    [TestMethod]
    public void Validate_ReturnsTrue_WhenRepositoryFindsKey()
    {
        var guid = Guid.NewGuid();
        var keyString = guid.ToString();

        _keyRepositoryMock!.Setup(r => r.Exists(It.IsAny<Expression<Func<Keytest, bool>>>()))
            .Returns(true);

        var result = _keyStrategy!.Validate(keyString);
        Assert.IsTrue(result);
    }

    #endregion

    #endregion

}
