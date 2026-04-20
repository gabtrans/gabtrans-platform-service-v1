using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.DataTransfer.Infinitus;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class FxTransactionService(IEmailNotificationService emailService, IUserRepository userRepository, ISequenceService sequenceService, IInfinitusService infinitusService, IWalletRepository walletRepository, IEncryptionService encryptionService, IAccountRepository accountRepository, IFxRateLogRepository fxRateLogRepository, ISettlementRepository settlementRepository, IOneTimePasswordService oneTimePasswordService, IFxTransactionRepository fxTransactionRepository, ITransactionPinRepository transactionPinRepository) : IFxTransactionService
    {
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IFxRateLogRepository _fxRateLogRepository = fxRateLogRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;
        private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;
        private readonly IFxTransactionRepository _fxTransactionRepository = fxTransactionRepository;
        private readonly ITransactionPinRepository _transactionPinRepository = transactionPinRepository;

        public async Task<ApiResponse> TradeFxAsync(TradeFxRequest request, long userId)
        {
            var account = await _accountRepository.GetAccountAsync(userId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch account details"
                };
            }

            if (!string.Equals(account.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Account is not active"
                };
            }

            var fxRateLog = await _fxRateLogRepository.GetAsync(request.RateToken);
            if (fxRateLog is null)
            {
                return new ApiResponse
                {
                    Message = "Rate token does not exist"
                };
            }

            var expiredTimeInSeconds = DateTime.Now.Subtract(fxRateLog.CreatedAt).TotalSeconds;
            if (expiredTimeInSeconds > fxRateLog.ValidityInSeconds)
            {
                return new ApiResponse
                {
                    Message = "Rate token has expired"
                };
            }

            var transactionPin = await _transactionPinRepository.GetAsync(account.UserId);
            if (transactionPin == null)
            {
                return new ApiResponse
                {
                    Message = "pin details not found"
                };
            }

            if (!_encryptionService.VerifyHash(request.TransactionPin, transactionPin.NewPin))
            {
                return new ApiResponse
                {
                    Message = "Your transaction pin is incorrect"
                };
            }

            var wallet = await _walletRepository.GetAsync(account.Id, fxRateLog.FromCurrency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Invalid wallet selected"
                };
            }

            if (wallet.Balance < Math.Round(fxRateLog.FromAmount, 2))
            {
                return new ApiResponse
                {
                    Message = "You have insufficient balance"
                };
            }

            return await TradeFxAsync(fxRateLog, account.Id, wallet.Id);
        }

        public async Task<ApiResponse> TradeFxAsync(FxRateLog fxRateLog, long accountId, long walletId)
        {
            string reference = _sequenceService.Settlement(2);

            //Get balance for merchant wallet
            var tillWallet = await _walletRepository.GetAsync(StaticData.GabTransBusinessGLId, fxRateLog.ToCurrency);
            if (tillWallet is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to debit the till wallet"
                };
            }

            if (Convert.ToDecimal(tillWallet.Balance) < Math.Round(fxRateLog.ToAmount, 2))
            {
                return new ApiResponse
                {
                    Message = "Till wallet does not have enough balance"
                };
            }

            var settlement = new SettlementModel
            {
                Currency = fxRateLog.FromCurrency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = fxRateLog.FromAmount,
                TransactionType = TransactionTypes.Conversion,
                WalletId = walletId,
                Note = $"{reference}|{fxRateLog.ToCurrency}",
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            string debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (!string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            var fxTransaction = new FxTransaction
            {
                AccountId = accountId,
                ToAmount = fxRateLog.ToAmount,
                FromCurrency = fxRateLog.FromCurrency,
                ToCurrency = fxRateLog.ToCurrency,
                CreatedAt = DateTime.Now,
                Reference = reference,
                Status = TransactionStatuses.Pending,
                FromAmount = fxRateLog.FromAmount,
                ConversionDate = DateTime.Now,
              //  ExecutedRate = fxRateLog.Rate
            };
            bool insert = await _fxTransactionRepository.InsertAsync(fxTransaction);
            if (!insert)
            {
                await ReversalAsync(fxRateLog, accountId, walletId, reference);

                return new ApiResponse
                {
                    Message = "Unable to log the Fx trade"
                };
            }

            //Credit the till wallet
            settlement = new SettlementModel
            {
                Currency = fxRateLog.FromCurrency,
                AccountId = StaticData.GabTransBusinessGLId,
                Reference = $"FX{reference}",
                TransactionAmount = fxRateLog.FromAmount,
                TransactionType = TransactionTypes.Conversion,
                WalletId = tillWallet.Id,
                Note = $"{reference}|{fxRateLog.FromCurrency}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            //Debit the Till wallet
            settlement = new SettlementModel
            {
                Currency = fxRateLog.ToCurrency,
                AccountId = StaticData.GabTransBusinessGLId,
                Reference = $"FX{reference}",
                TransactionAmount = fxRateLog.ToAmount,
                TransactionType = TransactionTypes.Conversion,
                WalletId = tillWallet.Id,
                Note = $"{reference}|{fxRateLog.FromCurrency}",
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            //Credit the customer wallet
            settlement = new SettlementModel
            {
                Currency = fxRateLog.ToCurrency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = fxRateLog.ToAmount,
                TransactionType = TransactionTypes.Conversion,
                WalletId = walletId,
                Note = $"{reference}|{fxRateLog.ToCurrency}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            fxTransaction.Status = TransactionStatuses.Successful;
            await _fxTransactionRepository.UpdateAsync(fxTransaction);

            var user = await _userRepository.GetByAccountIdAsync(accountId);

            //Send email notification
            await _emailService.FxTradeAsync(new FxTradeNotification { FxRateLog = fxRateLog, Reference = reference, Status = TransactionStatuses.Successful, EmailAddress = user.EmailAddress });

            return new ApiResponse { Success = true, Message = "Fx trade processed successfully" };
        }

        public async Task<ApiResponse> ReversalAsync(FxRateLog fxRateLog, long accountId, long walletId, string reference)
        {
            var settlement = new SettlementModel
            {
                Currency = fxRateLog.FromCurrency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = fxRateLog.FromAmount,
                TransactionType = TransactionTypes.Conversion,
                WalletId = walletId,
                Note = $"{reference}|{fxRateLog.ToCurrency}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Fx trade failed"
                };
            }

            if (!string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            return new ApiResponse { Success = true, Message = "Unable to process Fx trade" };
        }

        public async Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction)
        {
            var transactions = await _fxTransactionRepository.GetAsync(queryTransaction);

            int totalCount = transactions.Count;

            transactions = transactions.Skip((queryTransaction.PageNumber - 1) * queryTransaction.PageSize).Take(queryTransaction.PageSize).ToList();

            return new PaginatedResponse(transactions, queryTransaction.PageNumber, queryTransaction.PageSize, totalCount, "Records retrieved successfully", true);
        }
    }
}
