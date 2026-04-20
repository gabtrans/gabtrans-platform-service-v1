using System.Net.Http.Headers;
using System.Net;
using System.Text;
using GabTrans.Application.Abstractions.Repositories;

namespace GabTrans.Api.Middlewares
{
    public class SwaggerBasicAuth : IMiddleware
    {
        private readonly IAuthRepository _accessRepository;
        public SwaggerBasicAuth(IAuthRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the credentials from request header
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];
                    // validate credentials
                    if (await _accessRepository.ValidateAsync(username, password))
                    {
                        await next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
                }
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
