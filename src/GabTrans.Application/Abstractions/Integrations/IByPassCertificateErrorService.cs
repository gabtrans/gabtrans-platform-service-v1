using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IByPassCertificateErrorService
    {
        void BypassCertificateError();
        bool TrustAllCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors);
    }
}
