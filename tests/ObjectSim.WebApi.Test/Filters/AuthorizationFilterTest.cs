using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using ObjectSim.WebApi.Filter;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace ObjectSim.WebApi.Test.Filters;

[TestClass]
public class AuthorizationFilterTest
{
    private AuthorizationFilterContext? _context;
    private AuthorizationFilter? _filter;
    private readonly Guid _key = Guid.NewGuid();

    [TestInitialize]
    public void Initialize()
    {
        _filter = new AuthorizationFilter(_key.ToString());

        var httpContext = new DefaultHttpContext();
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

    #endregion

    #region Success



    #endregion

    #endregion

}
