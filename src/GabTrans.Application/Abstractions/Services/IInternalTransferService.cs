using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IInternalTransferService
    {
        Task<ApiResponse> AccountValidationAsync(string accountNumber);
        Task<ApiResponse> SendMoneyAsync(InternalTransferRequest request, string country);
    }
}
