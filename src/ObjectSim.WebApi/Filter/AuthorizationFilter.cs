using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using ObjectSim.Security;

namespace ObjectSim.WebApi.Filter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationFilter()
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Invalid KeyStrat.",
                Message = "API Key Invalid or wrong."
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var apiKey = authorizationHeader.ToString();

        try
        {
            var keyService = context.HttpContext.RequestServices.GetRequiredService<ISecurityService>();

            if (!keyService.IsValidKey(apiKey))
            {
                context.Result = new ObjectResult(new
                {
                    InnerCode = "Invalid.",
                    Message = $"This is not a valid key: {apiKey}."
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
