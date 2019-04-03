using Microsoft.AspNetCore.Http;

namespace GMaster.Services
{
    public class Plans : Service
    {
        public Plans(HttpContext context, Parameters parameters) : base(context, parameters) { }
    }
}
