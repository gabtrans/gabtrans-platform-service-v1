using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface ILimitService
    {
        Task<ApiResponse> CreateAsync(LimitRequest request);
        Task<ApiResponse> UpdateAsync(LimitRequest request, long id);
        Task<ApiResponse> GetAsync(long accountId, long accountKycType, decimal amount);
    }
}
