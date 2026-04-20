using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class AccountRequestRepository(GabTransContext context, ILogService logService) : IAccountRequestRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<AccountRequest> DetailsAsync(long id)
        {
            return await _context.AccountRequests.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AccountRequest> DetailsByAccountAsync(long accountId)
        {
            return await _context.AccountRequests.AsNoTracking().Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AccountRequest>> GetAsync(string status)
        {
            return await _context.AccountRequests.AsNoTracking().Where(x => x.Status == status).ToListAsync();
        }

        //public async Task<IEnumerable<KycApprovalRequestModel>> GetAsync(KycApprovalsRequest request)
        //{
        //    DateTime fromDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(request.StartDate);
        //    DateTime toDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now : Convert.ToDateTime(request.EndDate);

        //    return await (from kr in _context.AccountRequests.AsNoTracking()
        //                  from u in _context.Users.AsNoTracking().Where(u => u.Id == kr.UserId).DefaultIfEmpty()
        //                  from a in _context.Accounts.AsNoTracking().Where(a => a.UserId == kr.UserId)
        //                  where
        //                  (kr.Status == request.Status || string.IsNullOrEmpty(request.Status))
        //                  && (string.IsNullOrEmpty(request.AccountName) || a.Name == request.AccountName) &&
        //                  (string.IsNullOrEmpty(request.EmailAddress) || u.EmailAddress == request.EmailAddress) &&
        //                  (string.IsNullOrEmpty(request.Status) || kr.Status == request.Status) &&
        //                  kr.CreatedAt >= fromDate && kr.CreatedAt <= toDate
        //                  select new KycApprovalRequestModel
        //                  {
        //                      UserId = kr.UserId,
        //                      Id = kr.Id,
        //                      AccountId = a == null ? 0 : a.Id,
        //                      Status = kr.Status,
        //                      Comment = kr.Comment,
        //                      CreatedAt = kr.CreatedAt,
        //                      EmailAddress = u.EmailAddress,
        //                      FullName = a.Name,
        //                      PhoneNumber = u.PhoneNumber,
        //                      TotalTransaction = _context.Settlements.Where(s => s.Type != TransactionTypes.Charges).Count(p => p.AccountId == a.Id),
        //                      Type = a.Type
        //                  }).ToListAsync();
        //}

        public async Task<bool> InsertAsync(AccountRequest accountRequest)
        {
            try
            {
                _context.AccountRequests.Add(accountRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRequestRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(AccountRequest accountRequest)
        {
            try
            {
                _context.AccountRequests.Update(accountRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRequestRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        //public async Task<bool> UpdateAsync(long userId, string status)
        //{
        //    try
        //    {
        //        var kycApprovalRequest = await _context.AccountRequests.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        //        if (kycApprovalRequest == null) return false;

        //        kycApprovalRequest.Status = status;
        //        _context.AccountRequests.Update(kycApprovalRequest);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError(nameof(AccountRequestRepository), nameof(UpdateAsync), ex);
        //    }

        //    return false;
        //}
    }
}
