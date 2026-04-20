using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class KycRequestRepository(GabTransContext context, ILogService logService) : IKycRequestRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<KycRequest> DetailsAsync(long id)
        {
            return await _context.KycRequests.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<KycRequest> DetailsByUserAsync(long userId)
        {
            return await _context.KycRequests.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<KycApprovalRequestModel>> GetAsync(KycApprovalsRequest request)
        {
            DateTime fromDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(request.StartDate);
            DateTime toDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now : Convert.ToDateTime(request.EndDate);

            return await (from kr in _context.KycRequests.AsNoTracking()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == kr.UserId).DefaultIfEmpty()
                          from b in _context.Businesses.AsNoTracking().Where(b => b.UserId == kr.UserId).DefaultIfEmpty()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.UserId == kr.UserId).DefaultIfEmpty()
                          from k in _context.Kycs.AsNoTracking().Where(k => k.UserId == kr.UserId)
                          where
                          (k.Type == request.Type || string.IsNullOrEmpty(request.Type))
                          && (string.IsNullOrEmpty(request.AccountName) || b.Name == request.AccountName || u.FirstName.Contains(request.AccountName, StringComparison.OrdinalIgnoreCase)) &&
                          (string.IsNullOrEmpty(request.EmailAddress) || u.EmailAddress == request.EmailAddress) &&
                          (string.IsNullOrEmpty(request.Status) || kr.Status == request.Status) &&
                          kr.CreatedAt >= fromDate && kr.CreatedAt <= toDate
                          select new KycApprovalRequestModel
                          {
                              UserId = kr.UserId,
                              Id = kr.Id,
                              AccountId = a == null ? 0 : a.Id,
                              Status = kr.Status,
                              Comment = kr.Comment,
                              CreatedAt = kr.CreatedAt,
                              EmailAddress = u.EmailAddress,
                              FullName = b == null ? $"{u.FirstName} {u.LastName}" : b.Name,
                              PhoneNumber = u.PhoneNumber,
                              TotalTransaction = a == null ? 0 : _context.Settlements.Where(s => s.Type != TransactionTypes.Charges).Count(p => p.AccountId == a.Id),
                              Type = k.Type
                          }).ToListAsync();
        }

        public async Task<bool> InsertAsync(KycRequest kycRequest)
        {
            try
            {
                _context.KycRequests.Add(kycRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycRequestRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(KycRequest kycRequest)
        {
            try
            {
                _context.KycRequests.Update(kycRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycRequestRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(long userId, string status)
        {
            try
            {
                var kycApprovalRequest = await _context.KycRequests.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (kycApprovalRequest == null) return false;

                kycApprovalRequest.Status = status;
                _context.KycRequests.Update(kycApprovalRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(KycRequestRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
