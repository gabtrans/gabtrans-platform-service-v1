using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;



namespace GabTrans.Application.Services
{
    public class WalletTransactionService(IWalletRepository walletRepository, ILimitRepository limitRepository, ISettlementRepository walletTransactionRepository) : IWalletTransactionService
    {
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly ILimitRepository _limitRepository = limitRepository;
        private readonly ISettlementRepository _walletTransactionRepository = walletTransactionRepository;

        public async Task<ApiResponse> CreditAsync(long accountId, string referenceNumber, string currency, decimal amount, decimal fee, long transactionTypeId, string category)
        {
            decimal pendingBalance = 0;

            var wallet = await _walletRepository.GetByCurrencyAsync(accountId, currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message="Wallet details not found"
                };
            }

            var limit = await _limitRepository.GetAsync(accountId,"");
            if (limit is null)
            {
                return new ApiResponse
                {
                    Message="Limit details not found"
                };
            }

            decimal expectedBalance = wallet.Balance + amount;
            //if (expectedBalance > limit.MaximumBalance && limit.AccountTypeKycId !=(long)AccountKycTypes.Premium) pendingBalance=expectedBalance;

            //string? process = await _walletTransactionRepository.ProcessTransactionAsync(accountId, wallet.Id, referenceNumber, currency, amount, fee, transactionTypeId,category, pendingBalance, false);
            //if (process is not null && process != "Successful")
            //{
            //    return new ApiResponse
            //    {
            //        Message="Unable to log wallet transaction"
            //    };
            //}

            return new ApiResponse { Success=true };
        }

        public async Task<ApiResponse> DebitAsync(long accountId, string referenceNumber, string currency, decimal amount, decimal fee, long transactionTypeId,string category)
        {
            decimal pendingBalance = 0;

            var wallet = await _walletRepository.GetByCurrencyAsync(accountId, currency);
            if (wallet is null)
            {
                return new ApiResponse
                {
                    Message="Wallet details not found"
                };
            }

            //string? process = await _walletTransactionRepository.ProcessTransactionAsync(accountId, wallet.Id, referenceNumber, currency, amount, fee, transactionTypeId,category, pendingBalance);
            //if (process is not null && process != "Successful")
            //{
            //    return new ApiResponse
            //    {
            //        Message="Unable to log wallet transaction"
            //    };
            //}

            return new ApiResponse { Success=true };
        }
    }
}
