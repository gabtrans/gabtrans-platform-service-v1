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
    public interface IFxTransactionRepository
    {
        Task<bool> InsertAsync(FxTransaction fxTransaction);
        Task<bool> UpdateAsync(FxTransaction fxTransaction);
        Task<FxTransactionModel> DetailsAsync(long id);
        Task<List<FxTransaction>> GetAsync(long accountId);
        Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction);
        Task<long> GetCurrencyConversionAsync();
        Task<List<SummaryValue>> RevenuesAsync();
    }
}
