using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class ApplicationSettings
    {
        public const string SupportMail = "Support Email";
        public const string Hotline = "HotLine";
        public const string SupportPhoneNo = "Support PhoneNo";
        public const string AppUrl = "AppUrl";
        public const string InvitationUrl = "InvitationUrl";
        public const string Logo = "Logo";
        public const string SessionTimeOut = "Session TimeOut";
        public const string Domain = "Domain";
        public const string Storage = "Storage";
        public const string OtpLifetime = "OTP Lifetime";
        public const string ConnectionString = "ConnectionString";
        public const string Jwtkey = "JwtKey";
        public const string JwtIssuer = "JwtIssuer";
        public const string JwtExpireMinutes = "JwtExpireMinutes";
        public const string RefreshTokenExpireMinutes = "RefreshTokenExpireMinutes";
        public const string BackendEmailAddress = "BackendEmailAddress";

        public const string GabTransAccountGLId = "GabTransAccountGLId";
        public const string GlobusBankScope = "GlobusBankScope";
        public const string GlobusBankTokenEndpoint = "GlobusTokenEndpoint";
        public const string GlobusBankGrantType = "GlobusGrantType";
        public const string GlobusPayoutAccount = "GlobusPayoutAccount";
        public const string GlobusCollectionAccount = "GlobusCollectionAccount";
        public const string GlobusAccountName = "GlobusAccountName";
        public const string GlobusExpiredTime = "VirtualAccExpiredMin";
        public const string GlobusCorporateAccount = "GlobusCorporateAccount";
        public const string GlobusLinkedAccount = "GlobusLinkedAccount";
    }
}
