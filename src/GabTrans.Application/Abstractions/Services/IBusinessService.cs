using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IBusinessService
    {
        Task<ApiResponse> CreateAsync(long userId, string name);
        Task<BusinessObject> DetailsByAccountIdAsync(long accountId);
        Task<ApiResponse> UpdateAsync(UpdateBusinessRequest request);
      //  Task<ApiResponse> UpdateAddressAsync(UpdateBusinessAddressRequest request, long userId);
       // Task<ApiResponse> UpdateDocumentAsync(UpdateBusinessDocumentRequest request, long userId);
      //  Task<ApiResponse> UpdateBusinessInformationAsync(UpdateBusinessInformationRequest request, long userId);
        // Task<ApiResponse> UpdateCountryOfOperationAsync(UpdateCountryOperationRequest request, long businessId);
       // Task<ApiResponse> UpdateGeneralInformationAsync(UpdateGeneralInformationRequest request, long userId);
        // Task<ApiResponse> UpdateMailingAddressAsync(UpdateMailingAddressRequest request, long businessId);
    }
}
