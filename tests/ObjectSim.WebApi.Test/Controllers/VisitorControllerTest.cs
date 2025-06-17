using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Examples;
using ObjectSim.WebApi.Controllers;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class VisitorControllerTest
{
    private Mock<IExampleService> _exampleServiceMock = null!;
    private VisitorController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _exampleServiceMock = new Mock<IExampleService>();
        _controller = new VisitorController(_exampleServiceMock.Object);
    }

    [TestMethod]
    public void CreateExamples_WhenCalled_ShouldReturnOkAndInvokeService()
    {
        _exampleServiceMock
            .Setup(s => s.CreateExample())
            .Verifiable();

        var result = _controller.CreateExamples() as OkResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        _exampleServiceMock.Verify(s => s.CreateExample(), Times.Once);
    }

    [TestMethod]
    public void CreateExamples_WhenServiceThrowsException_ShouldPropagateException()
    {
        _exampleServiceMock
            .Setup(s => s.CreateExample())
            .Throws(new InvalidOperationException("internal error"));

        Action act = () => _controller.CreateExamples();

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("internal error");
    }
}
