using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using System.Net;

namespace GabTrans.Application.Services
{
    public class FeeService(ILogService logService, IAuditRepository auditRepository, IFeeRepository feeRepository) : IFeeService
    {
        private readonly ILogService _logService = logService;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly IFeeRepository _feeRepository = feeRepository;

        public async Task<ApiResponse> CreateAsync(FeeRequest request)
        {
            var exisitngFee = await _feeRepository.GetByCurrencyAsync(request.Currency, request.TransactionType);

            if (request.AccountId > 0) exisitngFee = await _feeRepository.GetByCurrencyAsync(request.Currency, request.TransactionType, request.AccountId);
            if (exisitngFee is not null)
            {
                exisitngFee.Rate = request.Rate;
                exisitngFee.CappedValue = request.CappedAmount;
                await _feeRepository.UpdateAsync(exisitngFee);

                return new ApiResponse
                {
                    Success = true,
                    Message = "Fee updated successfully"
                };
            }

            var fee = new Fee
            {
                AccountId = request.AccountId,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                CappedValue = request.CappedAmount,
                Rate = request.Rate,
                TransactionType = request.TransactionType
            };

            bool insert = await _feeRepository.InsertAsync(fee);
            if (!insert)
            {
                return new ApiResponse
                {
                    Message = "Unable to insert the fee"
                };
            }

            return new ApiResponse { Success = true, Message = "Added fee successfully" };
        }

        public async Task<ApiResponse> UpdateAsync(FeeRequest request, long id)
        {
            var fee = await _feeRepository.DetailsAsync(id);
            if (fee is null)
            {
                return new ApiResponse
                {
                    Message = "No record found for the ID"
                };
            }


            fee.AccountId = request.AccountId;
            fee.UpdatedAt = DateTime.Now;
            fee.CappedValue = request.CappedAmount;
            fee.Rate = request.Rate;

            bool update = await _feeRepository.UpdateAsync(fee);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update the fee"
                };
            }

            return new ApiResponse { Success = true, Message = "Updated fee successfully" };
        }

        public async Task<decimal> GetAsync(long accountId, string transactionType, string currency, decimal amount)
        {
            decimal totalFee = 0;

            var fee = await _feeRepository.GetAsync(accountId, transactionType, currency);
            if (fee is null) return 0;

            if (fee.Rate == 0 && fee.CappedValue > 0) return fee.CappedValue;

            totalFee = amount / fee.Rate;
            if (totalFee > fee.CappedValue) return fee.CappedValue;

            return totalFee;
        }

        public async Task<decimal> GetAsync(long accountId, string transactionType, string currency, string methodType, decimal amount)
        {
            decimal totalFee = 0;

            var fee = await _feeRepository.GetAsync(accountId, transactionType, currency, methodType.ToLower());
            if (fee is null) return 0;

            if (fee.Rate == 0 && fee.CappedValue > 0) return fee.CappedValue;

            totalFee = amount / fee.Rate;
            if (totalFee > fee.CappedValue) return fee.CappedValue;

            return totalFee;
        }
    }
}
