using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ObjectSim.WebApi.Filter;

public sealed class ExceptionFilter : IExceptionFilter
{
    private static readonly Dictionary<Type, (HttpStatusCode StatusCode, string InnerCode)> ExceptionMappings = new()
    {
        { typeof(ArgumentNullException), (HttpStatusCode.BadRequest, "ArgumentNull") },
        { typeof(ArgumentException), (HttpStatusCode.BadRequest, "InvalidArgument") },
        { typeof(InvalidOperationException), (HttpStatusCode.BadRequest, "InvalidOperation") },
        { typeof(KeyNotFoundException), (HttpStatusCode.NotFound, "NotFound") }
    };

    public void OnException(ExceptionContext context)
    {

    }
}
