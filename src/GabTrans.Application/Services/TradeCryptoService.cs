using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
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
    public class TradeCryptoService(IFeeService feeService, ISequenceService sequenceService, IInfinitusService infinitusService, IWalletRepository walletRepository, IEncryptionService encryptionService, IAccountRepository accountRepository, ISettlementRepository settlementRepository, ICryptoTradeRepository cryptoTradeRepository, IOneTimePasswordService oneTimePasswordService, ITransactionPinRepository transactionPinRepository) : ITradeCryptoService
    {
        private readonly IFeeService _feeService = feeService;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly IInfinitusService _infinitusService = infinitusService;
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;
        private readonly ICryptoTradeRepository _cryptoTradeRepository = cryptoTradeRepository;
        private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;
        private readonly ITransactionPinRepository _transactionPinRepository = transactionPinRepository;

        public async Task<ApiResponse> CreateAsync(TradeCryptoRequest request, long userId)
        {
            var account = await _accountRepository.GetAccountAsync(userId);
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

            var wallet = await _walletRepository.GetAsync(account.Id, Currencies.USD);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message = "Invalid wallet selected"
                };
            }

            var fee = await _feeService.GetAsync(account.Id, TransactionTypes.Crypto, wallet.Currency, request.FromAmount);
            if (fee == 0)
            {
                return new ApiResponse
                {
                    Message = "No fee configured"
                };
            }

            if (wallet.Balance < Math.Round(request.FromAmount + fee, 2))
            {
                return new ApiResponse
                {
                    Message = "You have insufficient balance"
                };
            }

            return await CreateAsync(request, account.Id, wallet.Id, wallet.Uuid, fee);
        }

        public async Task<ApiResponse> CreateAsync(TradeCryptoRequest request, long accountId, long walletId, string uuid, decimal fee)
        {
            string reference = _sequenceService.Settlement(2);

            var settlement = new SettlementModel
            {
                Currency = Currencies.USD,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = request.FromAmount,
                TransactionType = TransactionTypes.Crypto,
                WalletId = walletId,
                Note = $"{reference}|{request.Asset}",
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

            if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, TransactionStatuses.Successful, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = debit
                };
            }

            settlement = new SettlementModel
            {
                Currency = Currencies.USD,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = fee,
                TransactionType = TransactionTypes.Charges,
                WalletId = walletId,
                Note = $"{reference}|{request.Asset}",
                DebitCreditIndicator = DebitCreditIndicators.Debit
            };
            debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            var wallet = await _walletRepository.GetAsync(accountId, Currencies.USD);

            var tradeCrypto = new CryptoTrade
            {
                AccountId = accountId,
                ToAmount = request.ToAmount,
                ToAsset = request.Asset,
                ToNetwork = request.Network,
                CreatedAt = DateTime.Now,
                Reference = reference,
                Status = TransactionStatuses.Pending,
                FromAmount = request.FromAmount,
                FromAsset = wallet.Asset,
                FromNetwork = wallet.Network
            };
            bool insert = await _cryptoTradeRepository.InsertAsync(tradeCrypto);
            if (!insert)
            {
                return new ApiResponse
                {
                    Message = "No details found for the recipient ID"
                };
            }

            var response = await _infinitusService.TradeAsync(tradeCrypto, accountId, uuid);
            if (response.Success)
            {
                await ReverseAsync(request, reference, accountId, walletId, fee);

                tradeCrypto.Status = TransactionStatuses.Failed;
                await _cryptoTradeRepository.UpdateAsync(tradeCrypto);
                return response;
            }

            return response;
        }

        public async Task<ApiResponse> ReverseAsync(TradeCryptoRequest request, string reference, long accountId, long walletId, decimal fee)
        {
            var settlement = new SettlementModel
            {
                Currency = Currencies.USD,
                AccountId = accountId,
                Reference = reference,
                TransactionAmount = request.FromAmount + fee,
                TransactionType = TransactionTypes.Crypto,
                WalletId = walletId,
                Note = $"{reference}|{request.Asset}",
                DebitCreditIndicator = DebitCreditIndicators.Credit
            };
            string debit = await _settlementRepository.ProcessAsync(settlement);
            if (string.IsNullOrEmpty(debit))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            if (!string.Equals(debit, "duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Payment failed"
                };
            }

            return new ApiResponse { Message = "Unable to trade crypto", Success = true };
        }

        public async Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction)
        {
            var trades = await _cryptoTradeRepository.GetAsync(queryTransaction);

            int totalCount = trades.Count;

            trades = trades.Skip((queryTransaction.PageNumber - 1) * queryTransaction.PageSize).Take(queryTransaction.PageSize).ToList();

            return new PaginatedResponse(trades, queryTransaction.PageNumber, queryTransaction.PageSize, totalCount, "Records retrieved successfully", true);
        }
    }
}
