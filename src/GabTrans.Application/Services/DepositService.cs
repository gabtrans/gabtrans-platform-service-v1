using Azure.Storage.Blobs.Models;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace GabTrans.Application.Services
{
    public class DepositService(ISmsNotificationService smsService, ILogService logService, IFeeService feeService, IEmailNotificationService emailService, ISettlementService settlementService, ICurrencyRepository currencyRepository, IUserRepository userRepository, ICountryRepository countryRepository, IAccountRepository accountRepository, IDepositRepository depositRepository, IInfinitusService infinitusService, IWalletRepository walletRepository, ISettlementRepository settlementRepository, IPendingDepositRepository pendingDepositRepository, IVirtualAccountRepository virtualAccountRepository) : IDepositService
    {
        private readonly ISmsNotificationService _smsService = smsService;
        private readonly ILogService _logService = logService;
        private readonly IFeeService _feeService = feeService;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly ISettlementService _settlementService = settlementService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IDepositRepository _depositRepository = depositRepository;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;
        private readonly IPendingDepositRepository _pendingDepositRepository = pendingDepositRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository = virtualAccountRepository;

        public async Task<ApiResponse> GetTransactionsAsync(GetTransactionRequest request)
        {
            var result = new ApiResponse();

            try
            {
                var transactions = await _depositRepository.GetTransactionsAsync(request);

                result.Success = true;

                result.Message = "Success";
                result.Data = transactions;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransferService", "GetTransactions", ex);
                result.Message = "Kindly try again later";
            }
            return result;
        }


        public async Task<FeeObject> GetFeeAsync(long transferId, decimal amount, string countryCode)
        {
            var feeObject = new FeeObject();

            //feeObject = countryCode switch
            //{
            //    Countries.Nigeria => await _feeRepository.GetTransferFeeAsync(transferId, amount, countryCode),
            //    _ => await _feeRepository.GetTransferRateAsync(transferId, countryCode),
            //};

            return feeObject;
        }

        public async Task ProcessAsync()
        {
            try
            {
                var pendingDeposits = await _pendingDepositRepository.GetAsync(TransactionStatuses.Pending);
                foreach (var pendingDeposit in pendingDeposits)
                {

                    try
                    {
                        var response = new ApiResponse();

                        if (string.Equals(pendingDeposit.Gateway, PaymentGateways.Infinitus, StringComparison.OrdinalIgnoreCase)) response = await ProcessInfinitusAsync(pendingDeposit);

                        if (string.Equals(pendingDeposit.Gateway, PaymentGateways.Globus, StringComparison.OrdinalIgnoreCase)) response = await ProcessGlobusAsync(pendingDeposit);

                        if (response.Success)
                        {
                            pendingDeposit.Status = TransactionStatuses.Processed;
                            await _pendingDepositRepository.UpdateAsync(pendingDeposit);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError(nameof(DepositService), nameof(ProcessAsync), ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(DepositService), nameof(ProcessAsync), ex);
            }
        }

        public async Task<ApiResponse> ProcessInfinitusAsync(PendingDeposit pendingDeposit)
        {

            try
            {
                var account = await _accountRepository.DetailsByUuidAsync(pendingDeposit.AccountUuid);
                if (account is null)
                {
                    return new ApiResponse
                    {
                        Message = "No account found"
                    };
                }

                var wallet = await _walletRepository.GetByCurrencyAsync(account.Id, pendingDeposit.Currency);
                if (wallet is null)
                {
                    return new ApiResponse
                    {
                        Message = "No wallet found for this account"
                    };
                }

                var fee = await _feeService.GetAsync(account.Id, TransactionTypes.Deposit, pendingDeposit.Currency, (decimal)pendingDeposit.Amount);
                //if (fee == 0)
                //{
                //    return new ApiResponse
                //    {
                //        Message = "No fee configured"
                //    };
                //}

                var deposit = new Deposit
                {
                    AccountId = account.Id,
                    Currency = pendingDeposit.Currency,
                    Amount = (decimal)pendingDeposit.Amount,
                    Fee = fee,
                    GatewayReference = pendingDeposit.SessionId,
                    GatewayResponse = pendingDeposit.AllData,
                    PayerAccountName = pendingDeposit.SenderAccountName,
                    Status = TransactionStatuses.Successful,
                    Reference = pendingDeposit.PaymentReference,
                    CreatedAt = DateTime.Now,
                    Narration = pendingDeposit.Narration,
                    ResponseMessage = "Successful",
                    SettledAmount = Math.Round((decimal)pendingDeposit.Amount - fee, 2)
                };
                long depositId = await _depositRepository.InsertAsync(deposit);
                if (depositId == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Unable to log deposit"
                    };
                }

                var response = await _settlementService.DepositAsync(new ProcessDeposit { AccountId = account.Id, Fee = fee, PendingDeposit = pendingDeposit });
                if (!response.Success)
                {
                    _logService.LogInfo(nameof(DepositService), nameof(ProcessInfinitusAsync), "Response from settlement:: " + JsonConvert.SerializeObject(response));

                    return new ApiResponse
                    {
                        Message = "Deposit failed"
                    };
                }

                var user = await _userRepository.GetDetailsByUserIdAsync(account.UserId);

                await _emailService.FundingAsync(user.EmailAddress, pendingDeposit.Currency, (decimal)pendingDeposit.Amount);

                return new ApiResponse { Success = true, Message = "Deposit processed successfully" };
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(DepositService), nameof(ProcessInfinitusAsync), ex);
            }

            return new ApiResponse { Message = "Error occurred, kdly try again later" };
        }

        public async Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction)
        {
            var deposits = await _depositRepository.GetAsync(queryTransaction);

            int totalCount = deposits.Count;

            deposits = deposits.Skip((queryTransaction.PageNumber - 1) * queryTransaction.PageSize).Take(queryTransaction.PageSize).ToList();

            return new PaginatedResponse(deposits, queryTransaction.PageNumber, queryTransaction.PageSize, totalCount, "Records retrieved successfully", true);
        }

        public async Task<ApiResponse> ProcessGlobusAsync(PendingDeposit pendingDeposit)
        {
            try
            {
                var virtualAccount = await _virtualAccountRepository.DetailsByNumberAsync(pendingDeposit.BeneficiaryAccount);
                if (virtualAccount is null)
                {
                    return new ApiResponse
                    {
                        Message = "No virtual account found"
                    };
                }

                var account = await _accountRepository.DetailsAsync(virtualAccount.AccountId);
                if (account is null)
                {
                    return new ApiResponse
                    {
                        Message = "No account found"
                    };
                }

                var wallet = await _walletRepository.GetByCurrencyAsync(account.Id, pendingDeposit.Currency);
                if (wallet is null)
                {
                    return new ApiResponse
                    {
                        Message = "No wallet found for this account"
                    };
                }

                var fee = await _feeService.GetAsync(account.Id, TransactionTypes.Deposit, pendingDeposit.Currency, (decimal)pendingDeposit.Amount);
                //if (fee == 0)
                //{
                //    return new ApiResponse
                //    {
                //        Message = "No fee configured"
                //    };
                //}

                var deposit = new Deposit
                {
                    AccountId = account.Id,
                    Currency = pendingDeposit.Currency,
                    Amount = (decimal)pendingDeposit.Amount,
                    Fee = fee,
                    GatewayReference = pendingDeposit.PaymentReference,
                    GatewayResponse = JsonConvert.SerializeObject(pendingDeposit.AllData),
                    PayerAccountName = pendingDeposit.SenderAccountName,
                    Status = TransactionStatuses.Successful,
                    Reference = pendingDeposit.PaymentReference,
                    SettledAmount = Math.Round((decimal)pendingDeposit.Amount - fee, 2),
                    CreatedAt = DateTime.Now,
                    PayerAccountNumber = pendingDeposit.SenderAccountNumber,
                    PayerBank = pendingDeposit.BankName,
                    Narration = pendingDeposit.Narration
                };
                long depositId = await _depositRepository.InsertAsync(deposit);
                if (depositId == 0)
                {
                    return new ApiResponse
                    {
                        Message = "Unable to process deposit"
                    };
                }

                var response = await _settlementService.DepositAsync(new ProcessDeposit { AccountId = account.Id, Fee = fee, PendingDeposit = pendingDeposit });
                if (!response.Success)
                {
                    _logService.LogInfo(nameof(DepositService), nameof(ProcessGlobusAsync), "Response from settlement:: " + JsonConvert.SerializeObject(response));

                    return new ApiResponse
                    {
                        Message = "Deposit failed"
                    };
                }

                var user = await _userRepository.GetDetailsByUserIdAsync(account.UserId);

                await _emailService.FundingAsync(user.EmailAddress, pendingDeposit.Currency, (decimal)pendingDeposit.Amount);

                return new ApiResponse { Success = true, Message = "Deposit processed successfully" };
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(DepositService), nameof(ProcessGlobusAsync), ex);
            }

            return new ApiResponse { Message = "Error occurred, kdly try again later" };
        }
    }
}
