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
    public interface IKycRequestRepository
    {
        Task<bool> UpdateAsync(long userId, string status);
        Task<bool> InsertAsync(KycRequest kycRequest);
        Task<bool> UpdateAsync(KycRequest kycRequest);
        Task<KycRequest> DetailsAsync(long id);
        Task<KycRequest> DetailsByUserAsync(long userId);
        Task<IEnumerable<KycApprovalRequestModel>> GetAsync(KycApprovalsRequest request);
    }
}
