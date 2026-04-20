using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IKycService
    {
        Task SubmitAsync();
        Task UpdateAsync();
       // Task<ApiResponse> UpdatePersonalAsync(UpdatePersonalRequest request, long userId);
        Task<ApiResponse> UpdateIdentityAsync(UpdateIdentityRequest request, long userId);
        Task<ApiResponse> UpdateEmploymentAsync(UpdateEmploymentRequest request, long userId);
        Task<ApiResponse> UpdateIncomeAsync(UpdateIncomeRequest request, long userId);
        Task<ApiResponse> UpdateAddressAsync(UpdateAddressRequest request, long userId);
    }
}
