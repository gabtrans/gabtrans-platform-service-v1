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
    public interface ITransferRepository
    {
        Task<Transfer> DetailsAsync(long id);
        Task<Transfer> DetailsAsync(string reference);
        Task<TransferModel> DetailsByIdAsync(long id);
        Task<IEnumerable<Transfer>> GetByStatusAsync(string status);
        Task<IEnumerable<Transfer>> GetAsync(string status, string gateway);
        Task<bool> InsertAsync(Transfer transfer);
        Task<bool> UpdateAsync(Transfer transfer);
        Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction);
        Task<long> GetFundTransferAsync();
        Task<List<IdNameObject>> GetBankAccountTypesAsync();
        Task<AccountType> GetEntityAsync(string name);
        Task<List<IdNameObject>> GetReasonsAsync();
        Task<PaymentReason> GetReasonAsync(string name);
        Task<List<SummaryValue>> RevenuesAsync();
        Task<List<TransactionModel>> GetAsync(QueryTransfer queryTransfer);
    }
}
