namespace GabTrans.Api.Middlewares
{
    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerBasicAuth>();
        }
    }
}
