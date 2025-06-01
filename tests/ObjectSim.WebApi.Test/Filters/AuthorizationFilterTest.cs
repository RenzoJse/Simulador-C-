using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ObjectSim.WebApi.Filter;

namespace ObjectSim.WebApi.Test.Filters;

[TestClass]
public class AuthorizationFilterTest
{
    private AuthorizationFilterContext _context;
    private AuthorizationFilter _filter;

    [TestMethod]
    public void OnAuthorization_WhenKeyIsInvalid_ThrowsException()
    {
        _filter.OnAuthorization(_context);

        var answer = _context!.Result;
        answer.Should().NotBeNull();

        var response = answer as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

}
