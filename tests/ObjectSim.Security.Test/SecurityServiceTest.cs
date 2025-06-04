using Moq;
using ObjectSim.Security.Strategy;

namespace ObjectSim.Security.Test;

[TestClass]
public class SecurityServiceTest
{
    private Mock<IValidationStrategy>? _strategyMock;
    private SecurityService? _service;

    [TestInitialize]
    public void Initialize()
    {
        _strategyMock = new Mock<IValidationStrategy>(MockBehavior.Strict);
        _service = new SecurityService([_strategyMock.Object]);
    }

    #region IsValidKey

    #region Error

    [TestMethod]
    public void IsValidKey_WhenStrategyDoesNotMatch_ThrowsArgumentException()
    {
        const string key = "no-match";
        _strategyMock!.Setup(s => s.WhichStrategy(key)).Returns(false);

        Assert.ThrowsException<ArgumentException>(() => _service!.IsValidKey(key));
    }

    #endregion

    #region Success

    [TestMethod]
    public void IsValidKey_WhenStrategyMatches_ReturnsValidationResult()
    {
        var guidKey = Guid.NewGuid().ToString();
        _strategyMock!.Setup(s => s.WhichStrategy(guidKey)).Returns(true);
        _strategyMock.Setup(s => s.Validate(guidKey)).Returns(true);

        var result = _service!.IsValidKey(guidKey);

        Assert.IsTrue(result);
    }

    #endregion

    #endregion


}
