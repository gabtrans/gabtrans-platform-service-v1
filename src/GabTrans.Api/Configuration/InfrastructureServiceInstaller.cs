using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using GabTrans.Api.Middlewares;
using GabTrans.Application;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Interfaces.Integrations;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure;
using GabTrans.Infrastructure.Data;
using GabTrans.Infrastructure.Integrations;
using GabTrans.Infrastructure.Logging;
using GabTrans.Infrastructure.Repositories;
using GabTrans.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;



namespace GabTrans.Api.Configuration
{
    public class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GabTransContext>(options => options.UseNpgsql(StaticData.ConnectionStrings!, b => b.MigrationsAssembly(typeof(GabTransContext).Assembly.FullName)), ServiceLifetime.Scoped);

            services.AddSingleton(options => new BlobServiceClient(StaticData.StorageConnectionStrings));

            //QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IAuditRepository, AuditRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ISequenceRepository, SequenceRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOneTimePasswordRepository, OneTimePasswordRepository>();
            services.AddTransient<IRecipientRepository, RecipientRepository>();
            services.AddTransient<IFeeRepository, FeeRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            services.AddTransient<IDepositRepository, DepositRepository>();
            services.AddTransient<IKycRepository, KycRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<IWebhookRepository, WebhookRepository>();
            services.AddTransient<IVirtualAccountRepository, VirtualAccountRepository>();
            services.AddTransient<IQuickInsightRepository, QuickInsightRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IBusinessRepository, BusinessRepository>();
            services.AddTransient<IRepresentativeRepository, RepresentativeRepository>();
            services.AddTransient<ISettlementRepository, SettlementRepository>();
            services.AddTransient<ISignUpRepository, SignUpRepository>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<ISignInRepository, SignInRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<ILimitRepository, LimitRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<ITwoFactorAuthRepository, TwoFactorAuthRepository>();
            services.AddTransient<ITransactionPinRepository, TransactionPinRepository>();
            services.AddTransient<IAccountRequestRepository, AccountRequestRepository>();
            services.AddTransient<IInvitationRepository, InvitationRepository>();
            services.AddTransient<IBusinessTeamRepository, BusinessTeamRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<ICryptoTradeRepository, CryptoTradeRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IFxTransactionRepository, FxTransactionRepository>();
            services.AddTransient<IDisputeRepository, DisputeRepository>();
            services.AddTransient<IFxRateAuditRepository, FxRateAuditRepository>();
            services.AddTransient<IFxRateLogRepository, FxRateLogRepository>();
            services.AddTransient<IFxRateRepository, FxRateRepository>();
            services.AddTransient<IPendingDepositRepository, PendingDepositRepository>();
            services.AddTransient<ITransferProviderRepository, TransferProviderRepository>();
            services.AddTransient<IKycRequestRepository, KycRequestRepository>();


            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(3);
            var noOp = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            services.AddHttpClient<IFileStackClientIntegration, FileStackClientIntegration>()
           .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
           .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddTransient<IFileStackClientIntegration, FileStackClientIntegration>();

            services.AddHttpClient<IInfinitusClientIntegration, InfinitusClientIntegration>()
           .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
           .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddTransient<IInfinitusClientIntegration, InfinitusClientIntegration>();

            services.AddHttpClient<IMailClientIntegration, MailClientIntegration>()
            .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
            .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddTransient<IMailClientIntegration, MailClientIntegration>();

            services.AddHttpClient<IAzureFileClientIntegration, AzureFileClientIntegration>()
            .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
            .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddTransient<IAzureFileClientIntegration, AzureFileClientIntegration>();

            services.AddHttpClient<IGlobusBankClientIntegration, GlobusBankClientIntegration>()
            .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOp)
            .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddTransient<IGlobusBankClientIntegration, GlobusBankClientIntegration>();
        }
    }
}
