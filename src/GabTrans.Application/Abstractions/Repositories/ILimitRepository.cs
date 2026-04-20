using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ILimitRepository
    {
        Task<bool> InsertAsync(Limit limit);
        Task<bool> UpdateAsync(Limit limit);
        Task<List<LimitModel>> GetAsync(QueryLimit queryLimit);
        Task<Limit> DetailsByIdAsync(long id);
        Task<LimitModel> DetailsAsync(long id);
        Task<Limit> GetAsync(long accountId, string type);
        Task<Limit> DetailsByAccountTypeAsync(long accountTypeId);
        Task<Limit> GetByCurrencyAsync(string currency, string type);
        Task<Limit> GetByCurrencyAsync(string currency, string type, long accountId);
        Task<Limit> GetAsync(long accountId, string type, string currency);
    }
}

