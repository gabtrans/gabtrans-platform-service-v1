using FluentValidation;
using GabTrans.Api.Middlewares;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Services;
using GabTrans.Infrastructure.Encryption;
using GabTrans.Infrastructure.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Infrastructure.Notification;
using GabTrans.Application.Abstractions.Notification;


namespace GabTrans.Api.Configuration
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<ISequenceService, SequenceService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IOneTimePasswordService, OneTimePasswordService>();
            services.AddTransient<IByPassCertificateErrorService, ByPassCertificateErrorService>();
            services.AddTransient<IFeeService, FeeService>();
            services.AddTransient<IReceiptService, ReceiptService>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<ISignUpService, SignUpService>();
            services.AddTransient<IAmountToWordService, AmountToWordService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IVirtualAccountService, VirtualAccountService>();
            services.AddTransient<IComplianceService, ComplianceService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IBusinessService, BusinessService>();
             services.AddTransient<IRsaEncryptionService, RsaEncryptionService>();
            services.AddTransient<IRepresentativeService, RepresentativeService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IWalletTransactionService, WalletTransactionService>();
            services.AddTransient<IEmailNotificationService, EmailNotificationService>();
            services.AddTransient<ISmsNotificationService, SmsNotificationService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ISettlementService, SettlementService>();
            services.AddTransient<ILimitService, LimitService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<SwaggerBasicAuth, SwaggerBasicAuth>();
            services.AddTransient<IInfinitusService, InfinitusService>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IComplianceService, ComplianceService>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<IKycService, KycService>();
            services.AddTransient<ITradeCryptoService, TradeCryptoService>();
            services.AddTransient<ITransactionPinService, TransactionPinService>();
            services.AddTransient<IFxTransactionService, FxTransactionService>();
            services.AddTransient<IDisputeService, DisputeService>();
            services.AddTransient<IAzureStorageService, AzureStorageService>();
            services.AddTransient<IGlobusBankService, GlobusBankService>();
            services.AddTransient<IAccountRequestService, AccountRequestService>();
            services.AddTransient<IGRemitService, GRemitService>();
            services.AddTransient<IBankTransferService, BankTransferService>();

            services.AddValidatorsFromAssemblyContaining<Program>();
        }
    }
}
