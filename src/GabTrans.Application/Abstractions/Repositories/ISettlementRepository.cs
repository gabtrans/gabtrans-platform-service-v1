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
    public interface ISettlementRepository
    {
        Task<decimal> DailyCumulativeAsync(long accountId);
        Task<Settlement> DetailsAsync(long id);
        Task<Settlement> DetailsAsync(string referenceNumber);
        Task<List<Settlement>> GetAsync(GetTransactionHistoryRequest request);
        Task<string> ProcessAsync(SettlementModel settlementModel);
        Task<long> GetTotalTransactionAsync();
        Task<Settlement> DetailsAsync(string referenceNumber, string indicator);

    }
}
