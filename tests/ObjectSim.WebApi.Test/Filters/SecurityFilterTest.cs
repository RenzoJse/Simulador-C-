using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ObjectSim.WebApi.Test.Filters;

[TestClass]
public class SecurityFilterTest
{
    private SecurityFilterContext? _context;
    private SecurityFilter _filter;

    [TestMethod]
    public void OnAuthorization_WhenKeyIsInvalid_ThrowsException()
    {
        _filter.OnAuthorization();

        var answer = _context!.Result;
        answer.Should().NotBeNull();

        var response = answer as ObjectResult;
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
    }

}
