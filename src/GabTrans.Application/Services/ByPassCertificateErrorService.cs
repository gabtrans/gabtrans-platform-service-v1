using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using GabTrans.Application.Abstractions.Integrations;

namespace GabTrans.Application.Services
{
    public class ByPassCertificateErrorService : IByPassCertificateErrorService
    {
        public void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        public bool TrustAllCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
