using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NguyenHoangSon_NET1707_A02.Data
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if(path.HasValue && path.Value.StartsWith("/") && !path.Value.StartsWith("/Auths/Login"))
            {
                if(httpContext.Session.GetString("Username") == null)
                {
                    httpContext.Response.Redirect("/Auths/Login");
                }
            }
            if(path.Value.StartsWith("/auths/login"))
            {
                if(httpContext.Session.GetString("Username") != null)
                {
                    httpContext.Response.Redirect("/");
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
