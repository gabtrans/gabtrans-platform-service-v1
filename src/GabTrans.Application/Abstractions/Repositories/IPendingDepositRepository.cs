using System;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Repositories;

public interface IPendingDepositRepository
{
        Task<IEnumerable<PendingDeposit>> GetAsync();
        Task<IEnumerable<PendingDeposit>> GetAsync(string status);
        Task<PendingDeposit> DetailsAsync(long id);
        Task<PendingDeposit> DetailsByReferenceAsync(string reference);
        Task<bool> InsertAsync(PendingDeposit pendingDeposit);
        Task<bool> UpdateAsync(PendingDeposit pendingDeposit);
}
