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
    public interface ITransferService
    {
        Task ProcessAsync();
        Task QueryStatusAsync();
        Task<ApiResponse> ProcessAsync(Transfer transfer);
        Task<ApiResponse> ReverseAsync(Transfer transfer);
        Task<ApiResponse> CreateAsync(TransferRequest request, long userId);
    }
}
