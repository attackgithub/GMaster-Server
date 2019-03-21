using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace GMaster.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class DeveloperKeyValidatorMiddleware
    {
        private readonly RequestDelegate _next;
        public DeveloperKeyValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var remoteIpAddress = httpContext.Connection.RemoteIpAddress;

                if (httpContext.Request.Path.StartsWithSegments("/api"))
                {
                    var queryString = httpContext.Request.Query;
                    StringValues developerKey;
                    queryString.TryGetValue("key", out developerKey);

                    if (httpContext.Request.Method != "POST")
                    {
                        httpContext.Response.StatusCode = 405;         
                        await httpContext.Response.WriteAsync("Method Not Allowed");
                        return;
                    }

                    if (developerKey.Count == 0)
                    {
                        //missing query string
                        httpContext.Response.StatusCode = 400;           
                        await httpContext.Response.WriteAsync("'key' query string is required");
                        return;
                    }
                    else
                    {
                        //check developer key if it is valid
                        string[] paths = httpContext.Request.Path.Value.Split('/');
                        var userId = Query.DeveloperKeys.Authenticate(developerKey.ToString());
                        if(userId.HasValue == false)
                        {
                            httpContext.Response.StatusCode = 401;
                            await httpContext.Response.WriteAsync("Invalid authentication key");
                        }
                    }
                }
                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DeveloperKeyValidatorMiddleware>();
        }
    }
}