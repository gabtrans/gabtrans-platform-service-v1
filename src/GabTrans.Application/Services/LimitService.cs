using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class LimitService(ILimitRepository limitRepository, ISettlementRepository settlementRepository) : ILimitService
    {
        private readonly ILimitRepository _limitRepository = limitRepository;
        private readonly ISettlementRepository _settlementRepository = settlementRepository;

        public async Task<ApiResponse> CreateAsync(LimitRequest request)
        {
            var exisitngFee = await _limitRepository.GetByCurrencyAsync(request.Currency, request.TransactionType);

            if (request.AccountId > 0) exisitngFee = await _limitRepository.GetByCurrencyAsync(request.Currency, request.TransactionType, request.AccountId);
            if (exisitngFee is not null)
            {
                return new ApiResponse
                {
                    Message = "There is an exisiting fee"
                };
            }

            var limit = new Limit
            {
                AccountId = request.AccountId,
                CreatedAt = DateTime.Now,
                Currency = request.Currency,
                DailyCount = request.DailyCount,
                AccountType = request.AccountType,
                DailyCumulative = request.DailyCumulative,
                SingleCumulative = request.SingleCumulative,
                TransactionType = request.TransactionType
            };

            bool insert = await _limitRepository.InsertAsync(limit);
            if (!insert)
            {
                return new ApiResponse
                {
                    Message = "Unable to add the limit"
                };
            }

            return new ApiResponse { Success = true, Message = "Added limit successfully" };
        }

        public async Task<ApiResponse> UpdateAsync(LimitRequest request, long id)
        {
            var limit = await _limitRepository.DetailsByIdAsync(id);
            if (limit is null)
            {
                return new ApiResponse
                {
                    Message = "No record found for the ID"
                };
            }


            limit.AccountId = request.AccountId;
            limit.UpdatedAt = DateTime.Now;
            limit.Currency = request.Currency;
            limit.SingleCumulative=request.SingleCumulative;
            limit.DailyCumulative = request.DailyCumulative;
            limit.DailyCount = request.DailyCount;
            
            bool update = await _limitRepository.UpdateAsync(limit);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to update the limit"
                };
            }

            return new ApiResponse { Success = true, Message = "Updated limit successfully" };
        }

        public async Task<ApiResponse> GetAsync(long accountId, long accountKycType, decimal amount)
        {
            var limit = await _limitRepository.DetailsByAccountTypeAsync(accountKycType);
            if (limit is null)
            {
                return new ApiResponse
                {
                    Message = "Limit not found"
                };
            }

            //if (amount > limit.MaximumTransaction)
            //{
            //    return new ApiResponse
            //    {
            //        Message="Your account is not eligible to transact this amount"
            //    };
            //}

            decimal totalCumulative = await _settlementRepository.DailyCumulativeAsync(accountId);
            if (amount > (totalCumulative + amount))
            {
                return new ApiResponse
                {
                    Message = "Your account has exceeded daily cumulative"
                };
            }

            return new ApiResponse { Success = true };
        }
    }
}
