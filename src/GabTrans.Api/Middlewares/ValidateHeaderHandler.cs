using System;
namespace GabTrans.Api.Middlewares
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        public ValidateHeaderHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return response;
        }
    }
}

