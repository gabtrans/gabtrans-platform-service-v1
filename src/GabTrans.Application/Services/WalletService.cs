using GabTrans.Domain.Models;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;

namespace GabTrans.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly ICryptoTradeRepository _cryptoTradeRepository;
        private readonly IDepositRepository _depositRepository;
        private readonly ILogService _logService;
        private readonly IWalletRepository _walletRepository;

        public WalletService(ICryptoTradeRepository cryptoTradeRepository, IDepositRepository depositRepository, ILogService logService, IWalletRepository walletRepository)
        {
            _cryptoTradeRepository = cryptoTradeRepository;
            _depositRepository = depositRepository;
            _logService = logService;
            _walletRepository = walletRepository;
        }

        public async Task<bool> CreateAsync(long accountId, string currencyCode)
        {
            return true; //await _walletRepository.CreateAsync(accountId, currencyCode);
        }

        public async Task<decimal> GetCurrentBalanceAsync(long accountId, string currency)
        {
            return await _walletRepository.GetCurrentBalanceAsync(accountId, currency);
        }

        public async Task<List<BalanceObject>> TotalBalancesAsync(long accountId)
        {
            return await _walletRepository.TotalBalancesAsync(accountId);
        }

    }
}
