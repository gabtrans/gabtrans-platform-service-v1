using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GabTrans.Infrastructure.Repositories
{
    public static class ApplicationRepository
    {

        public async static Task LoadAppInfoAsync()
        {
            StaticData.ConnectionStrings = ConnectionString.Get();

            using var _context = new GabTransContext();

            //StaticData.Countries = await _context.Countries.ToListAsync();

            var integrations = await _context.Integrations.ToListAsync();
            var messageCredentials = await _context.MessageCredentials.ToListAsync();
            var applicationSettings = await _context.ApplicationSettings.ToListAsync();

            StaticData.Banks = await _context.Banks.ToListAsync();

            StaticData.GRemitBanks = await _context.GRemitBanks.ToListAsync();

            StaticData.GremitAccounts = await _context.GremitAccounts.ToListAsync();

            StaticData.Logo = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.Logo).Value;

            StaticData.AppUrl = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.AppUrl).Value;

            StaticData.OtpLifetime = long.Parse(applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.OtpLifetime).Value);

            StaticData.Jwtkey = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.Jwtkey).Value;

            StaticData.JwtIssuer = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.JwtIssuer).Value;

            StaticData.InvitationLink = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.InvitationUrl).Value;

            StaticData.RefreshTokenExpireMinutes = Convert.ToInt16(applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.RefreshTokenExpireMinutes).Value);

            StaticData.JwtExpireMinutes = Convert.ToInt16(applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.JwtExpireMinutes).Value);

            var infinitusPay = integrations.FirstOrDefault(x => x.Name == Domain.Constants.Integrations.InfinitusPay);
            StaticData.InfintusURL = infinitusPay.BaseUrl;
            StaticData.InfintusApiKey = infinitusPay.SecretKey;

            var smtp = messageCredentials.FirstOrDefault(x => x.Name == MessageCredentials.SMTP);
            StaticData.SmtpHost = smtp.Host;
            StaticData.SmtpDisplayName = smtp.DisplayName;
            StaticData.SmtpPassword = smtp.Password;
            StaticData.SmtpPort = Convert.ToInt16(smtp.Port);
            StaticData.SmtpSender = smtp.Sender;
            StaticData.SmtpEncryption = smtp.Encryption;
            StaticData.SmtpUsername = smtp.Username;

            StaticData.StorageContainer = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.Storage).Value;

            StaticData.StorageConnectionStrings = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.ConnectionString).Value;

            StaticData.BackendEmailAddress = applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.BackendEmailAddress).Value;

            StaticData.GabTransAccountGLId = Convert.ToInt64(applicationSettings.FirstOrDefault(x => x.Name == ApplicationSettings.GabTransAccountGLId).Value);

            //GlobusBank
            var globusTokenEndpoint = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusBankTokenEndpoint).FirstOrDefault();
            StaticData.GlobusBankTokenEndpoint = globusTokenEndpoint.Value;

            var globusScope = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusBankScope).FirstOrDefault();
            StaticData.GlobusBankScope = globusScope.Value;

            var globusGrantType = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusBankGrantType).FirstOrDefault();
            StaticData.GlobusBankGrantType = globusGrantType.Value;

            // var globusPayout = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusPayoutAccount).FirstOrDefault();
            // StaticData.GlobusPayoutAccount = globusPayout.Value;

            var globusAccountName = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusAccountName).FirstOrDefault();
            StaticData.GlobusAccountName = globusAccountName.Value;

            var globusLinkedPartnerAccount = applicationSettings.Where(x => x.Name == ApplicationSettings.GlobusLinkedAccount).FirstOrDefault();
            StaticData.GlobusLinkedAccount = globusLinkedPartnerAccount.Value;

            var globusBank = integrations.FirstOrDefault(x => x.Name == Domain.Constants.Integrations.GlobusBank);
            StaticData.GlobusBankBaseURL = globusBank.BaseUrl;
            StaticData.GlobusBankSecretKey = globusBank.SecretKey;
            StaticData.GlobusBankClientId = globusBank.UserName;
        }
    }
}
