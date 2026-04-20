using GabTrans.Application.DataTransfer;
using System;

namespace GabTrans.Application.Abstractions.Services;

public interface ITransactionPinService
{
    Task<ApiResponse> CreateAsync(AuthorizationPinRequest request, long userId);
    Task<ApiResponse> UpdateAsync(UpdateAuthorizationPinRequest request, long userId);
}
