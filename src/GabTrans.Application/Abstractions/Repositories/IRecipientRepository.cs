using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IRecipientRepository
    {
        Task<bool> InsertAsync(TransferRecipient transferRecipient);
        Task<long> CreateAsync(TransferRecipient transferRecipient);
        Task<bool> DeleteAsync(TransferRecipient transferRecipient);
        Task<TransferRecipient> DetailsAsync(long id);
        Task<List<TransferRecipient>> GetAsync(long accountId);
        Task<List<TransferRecipient>> GetAsync(long? accountId, string type, string startDate, string endDate);
        Task<TransferRecipient> GetAsync(long accountId, string accountNumber, string iban, string currency, string accountType);
        Task<bool> UpdateAsync(TransferRecipient transferRecipient);
        Task<TransferRecipient> GetAsync(long accountId, string accountNumber, string currency, string accountType);
    }
}
