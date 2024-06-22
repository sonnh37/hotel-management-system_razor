using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace NguyenHoangSon_NET1707_A02.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private string EmailAddress = "";
        private string Role = "";

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value?.ToLower();

            if (path != null)
            {
                EmailAddress = httpContext.Session.GetString("Username");
                Role = httpContext.Session.GetString("Role");
                // nếu như ko phải link /auths... và ch đăng nhập thì chuyển login

                if (!path.StartsWith("/auths") && EmailAddress == null)
                {
                    httpContext.Response.Redirect("/auths/login");
                    return;
                }

                // customer 
                if ((path.StartsWith("/auths/login")
                    || path.StartsWith("/bookingreservations")
                    || path.StartsWith("/roominformations")
                    || path.StartsWith("/customers")
                    ) && EmailAddress != null && Role == null)
                {
                    // allow
                    if (path.StartsWith("/bookingreservations")
                        || path.StartsWith("/roominformations/details")
                        )
                    {
                        
                    } else
                    {
                        httpContext.Response.Redirect("/");
                        return;
                    }
                    
                }

                // admin
                if (path.StartsWith("/auths/login")
                     && EmailAddress != null && Role != null)
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
