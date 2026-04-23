using GabTrans.Domain.Entities;


namespace GabTrans.Domain.Models
{
    public static class StaticData
    {
        public static string ConnectionStrings { get; set; }
        public static string StorageContainer { get; set; }

        public static string BackendEmailAddress { get; set; }
        public static string StorageConnectionStrings { get; set; }
        //public static List<Country> Countries { get; set; }
        public static List<string> Languages { get; set; }
        public static string Logo { get; set; }
        public static string BudPayApi { get; set; }
        public static string AppUrl { get; set; }
        public static long SessionTimeOut { get; set; }
        public static string HotLine { get; set; }
        public static long OtpLifetime { get; set; }
        public static string InvitationLink { get; set; }

        public static string FileStackUrl { get; set; }
        public static string FileStackAppKey { get; set; }

        public static string SmtpUsername { get; set; }
        public static string SmtpPassword { get; set; }
        public static string SmtpHost { get; set; }
        public static int SmtpPort { get; set; }
        public static string SmtpDisplayName { get; set; }
        public static string SmtpEncryption { get; set; }
        public static string SmtpSender { get; set; }

        public static string InfintusApiKey { get; set; }
        public static string InfintusURL { get; set; }
        public static string InfintusAppId { get; set; }

        public static int OtpTimeOut { get; set; }
        public static string Jwtkey { get; set; }
        public static string JwtIssuer { get; set; }
        public static int JwtExpireMinutes { get; set; }
        public static int RefreshTokenExpireMinutes { get; set; }

        public static long GabTransAccountGLId { get; set; }

        public static string GlobusBankClientId { get; set; }
        public static string GlobusBankGrantType { get; set; }
        //public static string GlobusPayoutAccount { get; set; }
        //public static string GlobusCollectionAccount { get; set; }
        public static string GlobusCorporateAccount { get; set; }
        public static string GlobusLinkedAccount { get; set; }
        public static string GlobusAccountName { get; set; }
        public static string GlobusBankScope { get; set; }
        // public static string GlobusBankUsername { get; set; }
        public static string GlobusBankSecretKey { get; set; }
        public static string GlobusBankBaseURL { get; set; }
        public static string GlobusBankTokenEndpoint { get; set; }
        public static GlobusSessionToken GlobusSessionToken { get; set; }
        public static List<GremitAccount> GremitAccounts { get; set; }
        public static List<Bank> Banks { get; set; }
        public static List<GRemitBank> GRemitBanks { get; set; }
    }
}
