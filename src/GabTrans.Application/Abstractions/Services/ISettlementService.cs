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
    public interface ISettlementService
    {
        Task<ApiResponse> TransferAsync(ProcessTransfer transfer);
        Task<ApiResponse> DepositAsync(ProcessDeposit deposit);
        Task<ApiResponse> CreditAsync(long accountId, string currency, decimal amount, string countryCode, string referenceNumber, string narration);
    }
}
