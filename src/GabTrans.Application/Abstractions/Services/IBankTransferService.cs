using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IBankTransferService
    {
        Task<ApiResponse> ReverseAsync(Transfer transfer);
        Task<ApiResponse> TransferAsync(BankTransferRequest request, long accountId);
        Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction);
    }
}
