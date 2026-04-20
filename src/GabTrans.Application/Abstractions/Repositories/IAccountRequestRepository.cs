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
    public interface IAccountRequestRepository
    {
       // Task<bool> UpdateAsync(long userId, string status);
        Task<bool> InsertAsync(AccountRequest accountRequest);
        Task<bool> UpdateAsync(AccountRequest accountRequest);
        Task<AccountRequest> DetailsAsync(long id);
        Task<AccountRequest> DetailsByAccountAsync(long accountId);
        Task<IEnumerable<AccountRequest>> GetAsync(string status);
       // Task<IEnumerable<KycApprovalRequestModel>> GetAsync(KycApprovalsRequest request);
    }
}
