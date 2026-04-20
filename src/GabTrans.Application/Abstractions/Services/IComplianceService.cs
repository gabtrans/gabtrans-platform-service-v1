using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IComplianceService
    {
        Task CreateAsync();
        Task<ApiResponse> SubmitApplicationAsync(Kyc kyc);
        Task<ApiResponse> CreateClientAsync(User user, string type, string businessName);
    }
}
