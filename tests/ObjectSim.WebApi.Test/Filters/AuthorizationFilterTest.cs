using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ObjectSim.Security;
using ObjectSim.WebApi.Filter;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace ObjectSim.WebApi.Test.Filters;

[TestClass]
public class AuthorizationFilterTest
{
    private AuthorizationFilterContext? _context;
    private AuthorizationFilter? _filter;
    private Mock<ISecurityService>? _securityServiceMock;
    private readonly Guid _validKey = Guid.NewGuid();

    [TestInitialize]
    public void Initialize()
    {
        _filter = new AuthorizationFilter();

        var httpContext = new DefaultHttpContext();
        var services = new ServiceCollection();
        _securityServiceMock = new Mock<ISecurityService>();
        services.AddSingleton(_securityServiceMock.Object);
        httpContext.RequestServices = services.BuildServiceProvider();

        _context = new AuthorizationFilterContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()), []);
    }

    #region OnAuthorization

    #region Error

    [TestMethod]
    public void OnAuthorization_WhenKeyIsInvalid_ThrowsException()
    {
        _filter!.OnAuthorization(_context!);

        var answer = _context!.Result;
        answer.Should().NotBeNull();

        var response = answer as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public void OnAuthorization_WhenKeyIsMissing_ReturnsUnauthorized()
    {
        _filter!.OnAuthorization(_context!);

        var response = _context!.Result as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public void OnAuthorization_WhenKeyIsInvalid_ReturnsForbidden()
    {
        const string key = "invalid-key";
        _context!.HttpContext.Request.Headers.Authorization = key;
        _securityServiceMock!.Setup(s => s.IsValidKey(key)).Returns(false);

        _filter!.OnAuthorization(_context!);

        var response = _context!.Result as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [TestMethod]
    public void OnAuthorization_WhenExceptionThrown_ReturnsInternalServerError()
    {
        const string key = "any-key";
        _context!.HttpContext.Request.Headers.Authorization = key;
        _securityServiceMock!.Setup(s => s.IsValidKey(key)).Throws(new Exception("fail"));

        _filter!.OnAuthorization(_context!);

        var response = _context!.Result as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    #endregion

    #region Success

    [TestMethod]
    public void OnAuthorization_WhenKeyIsValid_DoesNotSetResult()
    {
        _context!.HttpContext.Request.Headers.Authorization = _validKey.ToString();
        _securityServiceMock!.Setup(s => s.IsValidKey(_validKey.ToString())).Returns(true);

        _filter!.OnAuthorization(_context!);

        _context!.Result.Should().BeNull();
    }

    #endregion

    #endregion

}
