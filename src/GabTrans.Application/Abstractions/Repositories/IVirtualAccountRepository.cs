using GabTrans.Application.Abstractions.Logging;
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
    public interface IVirtualAccountRepository
    {
        Task<bool> InsertAsync(VirtualAccount virtualAccount);
        Task<bool> UpdateAsync(VirtualAccount virtualAccount);
        Task<VirtualAccount> DetailsAsync(long id);
        Task<VirtualAccount> GetAsync(long accountId);
        Task<VirtualAccountModel> GetByUserIdAsync(long userId);
        Task<VirtualAccount> DetailsByNumberAsync(string accountNumber);
        Task<VirtualAccount> DetailsByBankAsync(long accountId, string bankName);
        Task<VirtualAccount> GetAsync(long accountId, string currency);
        Task<VirtualAccount> GetAsync(long accountId, string currency, string accountNumber);
    }
}
