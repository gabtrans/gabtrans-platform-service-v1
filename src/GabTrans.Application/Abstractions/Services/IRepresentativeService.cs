using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IRepresentativeService
    {
        Task<bool> CreateAsync(long userId, long businessId, string companyNumber);
        Task<List<RepresentativeModel>> GetListAsync(long userId);
        Task<ApiResponse> UpdateAddressAsync(UpdateAddressRequest request, long userId);
        Task<ApiResponse> UpdateEmploymentAsync(UpdateEmploymentRequest request, long userId);
        Task<ApiResponse> UpdateIdentityAsync(UpdateIdentityRequest request, long userId);
        Task<ApiResponse> UpdateIncomeAsync(UpdateIncomeRequest request, long userId);
        Task<ApiResponse> UpdatePersonalAsync(UpdatePersonalRequest request, long userId);
    }
}
