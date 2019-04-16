using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace GMaster.Common.Middleware
{
    public class OptionsRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public OptionsRequestMiddleware(RequestDelegate next, IHostingEnvironment environment)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                BeginInvoke(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async void BeginInvoke(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("OK");
        }
    }

    public static class OptionsRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseOptionsRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsRequestMiddleware>();
        }
    }
}
