using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer;
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
    public interface IFeeRepository
    {
        Task<bool> InsertAsync(Fee fee);
        Task<List<FeeModel>> GetAsync(QueryFee queryFee);
        Task<Fee> GetAsync(long accountId, string transactionType, string currency);
        Task<Fee> GetAsync(long accountId, string transactionType, string currency, string methodType);
        Task<Fee> DetailsAsync(long id);
        Task<List<Fee>> GetAsync();
        Task<bool> UpdateAsync(Fee fee);
        Task<Fee> GetByCurrencyAsync(string currency, string type);
        Task<Fee> GetByCurrencyAsync(string currency, string type, long accountId);
    }
}
