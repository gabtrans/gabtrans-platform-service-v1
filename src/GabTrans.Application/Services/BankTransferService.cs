using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Interfaces.Services;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class BankTransferService(ILogService logService, IFeeService feeService, IGlobusBankService globusService, ISequenceService sequenceService, IInfinitusService infinitusService, IWalletRepository walletRepository, ITransferRepository transferRepository, IAccountRepository accountRepository, ICountryRepository countryRepository, IEncryptionService encryptionService, IUserRepository userRepository, IRecipientRepository recipientRepository, ISettlementRepository settlementRepository, ITransactionPinRepository transactionPinRepository, IVirtualAccountRepository virtualAccountRepository, IEmailNotificationService emailNotificationService) : IBankTransferService
    {
        private readonly ILogService _logService = logService;
        private readonly IFeeService _feeService = feeService;
        private readonly IGlobusBankService _globusService = globusService;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly ITransferRepository _transferRepository = transferRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRecipientRepository _recipientRepository = recipientRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;
        private readonly ITransactionPinRepository _transactionPinRepository = transactionPinRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository = virtualAccountRepository;
        private readonly IEmailNotificationService _emailNotificationService = emailNotificationService;

        public async Task<ApiResponse> TransferAsync(BankTransferRequest request, long accountId)
        {
            var transfer = await _transferRepository.DetailsAsync(request.Reference);
            if (transfer is not null)
            {
                return new ApiResponse
                {
                    Message = "Reference already exists"
                };
            }

            var account = await _accountRepository.DetailsAsync(accountId);
            if (account == null)
            {
                return new ApiResponse
                {
                    Message = "Account details not found"
                };
            }

            if (!string.Equals(account.Status, AccountStatuses.Active, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Account is not active"
                };
            }

            var wallet = await _walletRepository.GetAsync(account.Id, request.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Invalid wallet selected"
                };
            }

            if (string.Equals(request.Currency, Currencies.NGN, StringComparison.OrdinalIgnoreCase)) request.PaymentMethod = PaymentMethods.Local;

            var fee = await _feeService.GetAsync(account.Id, TransactionTypes.Transfer, request.Currency, request.PaymentMethod, request.Amount);
            //if (fee == 0)
            //{
            //    return new ApiResponse
            //    {
            //        Message = "No fee configured"
            //    };
            //}

            if (wallet.Balance < 2000000)
            {
                var user = await _userRepository.GetByAccountIdAsync(account.Id);

                await _emailNotificationService.LowBalanceAsync(user.EmailAddress, request.Currency, wallet.Balance);
            }

            if (wallet.Balance < Math.Round(request.Amount + fee, 2))
            {
                return new ApiResponse
                {
                    Message = "You have insufficient balance"
                };
            }

            var purpose = await _transferRepository.GetReasonAsync(request.Reason);
            if (purpose is null && !string.Equals(request.CountryCode, Countries.Nigeria, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Please supply a valid payment reason"
                };
            }

            return await TransferAsync(new BankTransfer { Account = account, BankTransferRequest = request, Fee = fee, Wallet = wallet });
        }

        public async Task<ApiResponse> TransferAsync(BankTransfer bankTransfer)
        {
            var wallet = bankTransfer.Wallet;
            var request = bankTransfer.BankTransferRequest;

            string reference = bankTransfer.BankTransferRequest.Reference;

            var centralWallet = await _walletRepository.GetAsync(StaticData.GabTransAccountGLId, request.Currency);
            if (centralWallet is null)
            {
                return new ApiResponse
                {
                    Message = "No wallet found for the account"
                };
            }

            var settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = wallet.AccountId,
                Reference = reference,
                TransactionAmount = request.Amount,
                TransactionType = TransactionTypes.Transfer,
                WalletId = wallet.Id,
                Note = $"{reference}|{request.Reason}",
                TransactionFee = bankTransfer.Fee,
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            string debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = wallet.AccountId,
                Reference = reference,
                TransactionAmount = bankTransfer.Fee,
                TransactionType = TransactionTypes.Charges,
                WalletId = wallet.Id,
                Note = $"{reference}|{request.Reason}",
                TransactionFee = 0,
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            if (bankTransfer.Fee > 0) debit = await _settlementRepository.ProcessAsync(settlement);

            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            var feeSettlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = centralWallet.AccountId,
                Reference = reference,
                TransactionAmount = bankTransfer.Fee,
                TransactionType = TransactionTypes.Charges,
                WalletId = centralWallet.Id,
                Note = $"{reference}|{request.Reason}",
                TransactionFee = 0,
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string feeCredit = await _settlementRepository.ProcessAsync(feeSettlement);
            if (string.IsNullOrEmpty(feeCredit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(feeCredit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(feeCredit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = feeCredit
                };
            }

            if (string.Equals(request.Currency, Currencies.NGN, StringComparison.OrdinalIgnoreCase) && request.BankCode == "000027") request.BankCode = "103";

            var recipient = await _recipientRepository.GetAsync(wallet.AccountId, request.AccountNumber, request.Currency, string.Empty);
            if (recipient is not null) return await ExistingRecipientAsync(bankTransfer, wallet.AccountId, recipient.Id, reference, bankTransfer.Fee, wallet.Id, request.AccountName);

            return await NewRecipientAsync(bankTransfer, wallet.AccountId, reference, bankTransfer.Fee, wallet.Id, request.AccountName);
        }

        public async Task<ApiResponse> NewRecipientAsync(BankTransfer bankTransfer, long accountId, string reference, decimal fee, long walletId, string accountName)
        {
            var request = bankTransfer.BankTransferRequest;

            var recipient = new TransferRecipient
            {
                AccountId = accountId,
                Country = request.CountryCode,
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                Currency = request.Currency,
                BankName = request.BankName,
                CreatedAt = DateTime.Now,
                Name = request.AccountName,
                PaymentMethod = request.PaymentMethod,
                RoutingNumber = request.BankCode,
                Type = request.AccountType
            };
            long receipientId = await _recipientRepository.CreateAsync(recipient);
            if (receipientId == 0)
            {
                return new ApiResponse
                {
                    Message = "Unable to register recipient"
                };
            }

            var transfer = new Transfer
            {
                AccountId = accountId,
                Amount = request.Amount,
                AmountPaid = request.Amount,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                Fee = fee,
                Gateway = request.Currency == Currencies.NGN ? PaymentGateways.Globus : PaymentGateways.Infinitus,
                PaymentMethod = request.PaymentMethod,
                Status = string.Equals(request.Currency, Currencies.NGN, StringComparison.OrdinalIgnoreCase) ? TransactionStatuses.Approved : TransactionStatuses.Pending,
                TransferRecipientId = receipientId,
                ProcessingStatus = TransactionStatuses.Pending,
                Reason = request.Reason,
                Reference = reference,
                MetaData = bankTransfer.BankTransferRequest.MetaData,
                //SupportingDocument = request.SupportingDocument,
                SourceCurrency = request.Currency
            };
            bool insert = await _transferRepository.InsertAsync(transfer);
            if (!insert)
            {
                await ReverseAsync(request, reference, accountId, walletId, fee);

                transfer.Status = TransactionStatuses.Failed;
                await _transferRepository.UpdateAsync(transfer);
                return new ApiResponse
                {
                    Message = "Unable to initiate transfer"
                };
            }

            // await _emailService.TransferRequestAsync(accountName, request.Currency, request.Amount, transfer.CreatedAt);

            return new ApiResponse { Success = true, Message = "Transfer successfully logged for processing", Data = reference };
        }

        public async Task<ApiResponse> ExistingRecipientAsync(BankTransfer bankTransfer, long accountId, long recipientId, string reference, decimal fee, long walletId, string accountName)
        {
            var request = bankTransfer.BankTransferRequest;

            var transfer = new Transfer
            {
                AccountId = accountId,
                Amount = request.Amount,
                AmountPaid = request.Amount,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                Fee = fee,
                Gateway = request.Currency == Currencies.NGN ? PaymentGateways.Globus : PaymentGateways.Infinitus,
                PaymentMethod = request.PaymentMethod,
                Status = string.Equals(request.Currency, Currencies.NGN, StringComparison.OrdinalIgnoreCase) ? TransactionStatuses.Approved : TransactionStatuses.Pending,
                TransferRecipientId = recipientId,
                ProcessingStatus = TransactionStatuses.Pending,
                Reason = request.Reason,
                Reference = reference,
                SourceCurrency = request.Currency,
                // SupportingDocument = request.SupportingDocument,
            };
            bool insert = await _transferRepository.InsertAsync(transfer);
            if (!insert)
            {
                await ReverseAsync(request, reference, accountId, walletId, fee);

                transfer.Status = TransactionStatuses.Failed;
                await _transferRepository.UpdateAsync(transfer);

                return new ApiResponse
                {
                    Message = "Unable to initiate transfer"
                };
            }

            // await _emailService.TransferRequestAsync(accountName, request.Currency, request.Amount, transfer.CreatedAt);

            return new ApiResponse { Success = true, Message = "Transfer successfully logged for processing", Data = reference };
        }


        public async Task<ApiResponse> ReverseAsync(BankTransferRequest request, string reference, long accountId, long walletId, decimal fee)
        {
            var settlement = new SettlementModel
            {
                Currency = request.Currency,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = request.Amount + fee,
                TransactionType = TransactionTypes.Transfer,
                WalletId = walletId,
                Note = $"{reference}|{request.Reason}",
                TransactionFee = fee,
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            return new ApiResponse { Success = true, Message = "Unable to process payment" };
        }

        public async Task<ApiResponse> ReverseAsync(Transfer transfer)
        {
            var wallet = await _walletRepository.GetAsync(transfer.AccountId, transfer.Currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "No wallet found for the account"
                };
            }

            var settlement = new SettlementModel
            {
                Currency = transfer.Currency,
                AccountId = transfer.AccountId,
                Reference = transfer.Reference,
                TransactionAmount = transfer.Amount + transfer.Fee,
                TransactionType = TransactionTypes.Transfer,
                WalletId = wallet.Id,
                Note = $"{transfer.Reference}|{transfer.Reason}",
                DebitCreditIndicator = DebitCreditIndicators.Credit,
                TransactionFee = transfer.Fee,
            };
            string credit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(credit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(credit, "duplicate", StringComparison.OrdinalIgnoreCase) && !string.Equals(credit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = credit
                };
            }

            transfer.Status = TransactionStatuses.Reversed;
            transfer.UpdatedAt = DateTime.Now;
            await _transferRepository.UpdateAsync(transfer);

            return new ApiResponse { Success = true, Message = "Unable to process payment" };
        }




        public async Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction)
        {
            var payouts = await _transferRepository.GetAsync(queryTransaction);

            int totalCount = payouts.Count;

            payouts = payouts.Skip((queryTransaction.PageNumber - 1) * queryTransaction.PageSize).Take(queryTransaction.PageSize).ToList();

            return new PaginatedResponse(payouts, queryTransaction.PageNumber, queryTransaction.PageSize, totalCount, "Records retrieved successfully", true);
        }
    }
}
