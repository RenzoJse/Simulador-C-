using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using ObjectSim.Security;

namespace ObjectSim.WebApi.Filter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilter(string key)
    : Attribute, IAuthorizationFilter
{

    private string Key { get; } = key;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid Key.",
                Message = "That is not a valid key."
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var token = authorizationHeader.ToString();

        try
        {
            var keyService = context.HttpContext.RequestServices.GetRequiredService<ISecurityService>();

            if (keyService.isValidKey(key))
            {
                context.Result = new ObjectResult(new
                {
                    InnerCode = "Invalid.",
                    Message = $"This is not a valid key: {Key}."
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
        }
        catch (Exception)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Internal error.",
                Message = "There was an internal error processing the request."
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

}
