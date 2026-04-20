using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ICryptoTradeRepository
    {
        Task<bool> InsertAsync(CryptoTrade cryptoTrade);
        Task<bool> UpdateAsync(CryptoTrade cryptoTrade);
        Task<CryptoTrade> DetailsAsync(long id);
        Task<List<CryptoTrade>> GetAsync(long accountId);
        Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction);
        Task<long> GetCryptoTradeAsync();
        Task<List<SummaryValue>> RevenuesAsync();
        Task<List<Asset>> GetAssetsAsync();
        Task<List<Network>> GetNetworksAsync();
    }
}
