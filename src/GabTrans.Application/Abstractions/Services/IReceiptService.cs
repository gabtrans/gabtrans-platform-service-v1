using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;


namespace GabTrans.Application.Abstractions.Services
{
    public interface IReceiptService
    {
        byte[]? GetTemplate(Receipt receipt);
        Task<ApiResponse> GenerateAsync(string referenceNumber);
    }
}
