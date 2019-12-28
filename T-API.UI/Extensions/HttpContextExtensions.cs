using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace T_API.UI.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetAntiforgeryToken(this HttpContext httpContext)
        {
            var antiforgery = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
            var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
            string fieldName = tokenSet.FormFieldName;
            string requestToken = tokenSet.RequestToken;
            
            return requestToken;
        }

        public static int GetNameIdentifier(this HttpContext httpContext)
        {
            var nameIdentifier =
                httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return Convert.ToInt32(nameIdentifier);
        }
    }
}