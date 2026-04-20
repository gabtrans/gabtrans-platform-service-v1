using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IAccountService
    {
        Task ProcessAsync();
        //Task OpenAccountAsync();
        Task<ApiResponse> OpenAccountAsync(long userId);
    }
}

