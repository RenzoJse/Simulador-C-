using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ObjectSim.WebApi.Filter;
[ExcludeFromCodeCoverage]
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
        var exception = context.Exception;
        var exceptionType = exception.GetType();

        (HttpStatusCode statusCode, var innerCode) = ExceptionMappings.TryGetValue(exceptionType, out var mapped)
            ? mapped
            : (HttpStatusCode.InternalServerError, "InternalError");

        context.Result = new ObjectResult(new
        {
            InnerCode = innerCode, exception.Message
        })
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}
