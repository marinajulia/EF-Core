using System;
using Microsoft.AspNetCore.Http;

namespace src.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext httpContext)
        {
            var tenant = httpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];

            return tenant;
        }
    }
}