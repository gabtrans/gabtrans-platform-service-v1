using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class SettlementService(ICountryRepository countryRepository, ISignUpRepository signUpRepository, IDepositRepository depositRepository, ICurrencyRepository currencyRepository, IAccountRepository accountRepository, IWalletTransactionService walletTransactionService, IDepositRepository transactionRepository, IWalletRepository walletRepository, ISettlementRepository settlementRepository) : ISettlementService
    {
        private readonly IDepositRepository _transactionRepository = transactionRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IDepositRepository _depositRepository = depositRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IWalletTransactionService _walletTransactionService = walletTransactionService;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;

        public async Task<ApiResponse> CreditAsync(long accountId, string currency, decimal amount, string countryCode, string referenceNumber, string narration)
        {
            var account = await _accountRepository.DetailsAsync(accountId);
            if (account is null)
            {
                return new ApiResponse
                {
                    Message = "Account details not found"
                };
            }

            referenceNumber = string.Concat("reversal_", referenceNumber);

            var walletTransaction = await _settlementRepository.DetailsAsync(referenceNumber);
            if (walletTransaction is not null)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = "Reference already exist"
                };
            }

            var deposit = new Deposit
            {
                Currency = currency,
                AccountId = accountId,
                Narration = narration,
                Reference = referenceNumber,
                Amount = amount,
                SettledAmount = amount,
                Fee = 0,
                // Country = countryCode,
                CreatedAt = DateTime.Now,
                // PaymentTypeId = PaymentTypes.Local,
                //Gateway = Enum.GetName(Gateways.BudPay),
                Status = TransactionStatuses.Successful,
                // PayerCountryCode = countryCode,
                // PaymentOptionId = (long)PaymentOptions.BANK_TRANSFER
            };

            long tranId = await _transactionRepository.InsertAsync(deposit);
            if (tranId == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to save transaction"
                };
            }

            //var result = await _walletTransactionService.CreditAsync(accountId, referenceNumber, currency, amount, 0, TransactionTypes.Reversal, WalletTransactionCategories.Funding);
            //if (!result.Success)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "Unable to process transaction"
            //    };
            //}

            return new ApiResponse { Success = true, Data = referenceNumber };
        }

        public async Task<ApiResponse> DepositAsync(ProcessDeposit deposit)
        {
            var pendingDeposit = deposit.PendingDeposit;

            var wallet = await _walletRepository.GetByCurrencyAsync(deposit.AccountId, pendingDeposit.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Wallet not found"
                };
            }

            var creditSettlement = new SettlementModel
            {
                Currency = pendingDeposit.Currency,
                AccountId = deposit.AccountId,
                Reference = pendingDeposit.PaymentReference,
                TransactionAmount = (decimal)pendingDeposit.Amount,
                TransactionType = TransactionTypes.Deposit,
                WalletId = wallet.Id,
                Note = $"Topup from {pendingDeposit.SenderAccountName}|{pendingDeposit.PaymentReference}",
                DebitCreditIndicator = DebitCreditIndicators.Credit,
                TransactionFee = deposit.Fee
            };
            string credit = await _settlementRepository.ProcessAsync(creditSettlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Unable to process settlement"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            if (deposit.Fee > 0)
            {
                //Debit the wallet
                var debitSettlement = new SettlementModel
                {
                    Currency = pendingDeposit.Currency,
                    AccountId = deposit.AccountId,
                    Reference = pendingDeposit.PaymentReference,
                    TransactionAmount = deposit.Fee,
                    TransactionType = TransactionTypes.Charges,
                    WalletId = wallet.Id,
                    Note = $"Topup from {pendingDeposit.SenderAccountName}|{pendingDeposit.PaymentReference}",
                    DebitCreditIndicator = DebitCreditIndicators.Debit,
                    TransactionFee = 0
                };
                string debit = await _settlementRepository.ProcessAsync(debitSettlement);
                if (string.IsNullOrEmpty(debit))
                {
                    return new ApiResponse
                    {
                        Message = "Unable to process settlement"
                    };
                }

                if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse
                    {
                        Message = debit
                    };
                }
            }

            return new ApiResponse { Success = true, Message = "Settlement processed successfully" };
        }

        public Task<ApiResponse> TransferAsync(ProcessTransfer transfer)
        {
            throw new NotImplementedException();
        }
    }
}
