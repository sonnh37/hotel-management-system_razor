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

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value?.ToLower();

            if (path != null)
            {
                // nếu như ko phải link /auths... và ch đăng nhập thì chuyển login
                if (!path.StartsWith("/auths") && httpContext.Session.GetString("Username") == null)
                {
                    httpContext.Response.Redirect("/auths/login");
                    return;
                }

                // customer 
                if ((path.StartsWith("/auths/login")
                    //|| path.StartsWith("/bookingreservations")
                    || path.StartsWith("/roominformations")
                    || path.StartsWith("/customers")
                    ) && httpContext.Session.GetString("Username") != null && httpContext.Session.GetString("Role") == null)
                {
                    httpContext.Response.Redirect("/");
                    return;
                }

                // admin
                if ((path.StartsWith("/auths/login")
                    ) && httpContext.Session.GetString("Username") != null && httpContext.Session.GetString("Role") != null)
                {
                    httpContext.Response.Redirect("/");
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await _next(httpContext);
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
