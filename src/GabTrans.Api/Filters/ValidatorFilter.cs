using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;

namespace GabTrans.Api.Filters
{
    public class ValidatorFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;
        private const string CustomApiKeyParam = "X-Auth-Signature";

        public ValidatorFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var requestBody = context.Arguments[0] as T;

            if(requestBody is null)
            {
                return context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            var validationResult = await _validator.ValidateAsync(requestBody);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.ToResponse());
            }

            return await next(context);
        }
    }
}
