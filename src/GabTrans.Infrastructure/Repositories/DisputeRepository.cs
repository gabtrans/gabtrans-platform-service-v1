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
    public class DisputeRepository(GabTransContext context, ILogService logService) : IDisputeRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<Dispute> DetailsAsync(long id)
        {
            return await _context.Disputes.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Dispute> DetailsAsync(string reference)
        {
            return await _context.Disputes.AsNoTracking().Where(x => x.Reference == reference).FirstOrDefaultAsync();
        }

        public async Task<List<DisputeModel>> GetAsync(GetDisputeRequest request)
        {
            DateTime startDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(request.StartDate);
            DateTime endDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now : Convert.ToDateTime(request.EndDate);

            return await (from d in _context.Disputes.AsNoTracking()
                          from s in _context.Settlements.AsNoTracking().Where(s => s.Reference == d.Reference && s.Type != TransactionTypes.Charges).DefaultIfEmpty()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == d.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where d.CreatedAt.Date >= startDate.Date && d.CreatedAt.Date <= endDate.Date
                          && (string.IsNullOrEmpty(request.EmailAddress) || u.EmailAddress == request.EmailAddress)
                            && (string.IsNullOrEmpty(request.Reference) || d.Reference == request.Reference)
                                 && (string.IsNullOrEmpty(request.Status) || d.Status == request.Status)
                          && (request.AccountId == 0 || request.AccountId == null || d.AccountId == request.AccountId)

                          select new DisputeModel
                          {
                              CreatedAt = a.CreatedAt,
                              AccountName = a.Name,
                              AccountId = d.AccountId,
                              Status = d.Status,
                              Reference = d.Reference,
                              EmailAddress = u.EmailAddress,
                              Comment = d.Comment,
                              Type = d.Type,
                              Id = d.Id,
                              UpdatedAt = a.CreatedAt,
                          }).OrderByDescending(d=>d.Id).ToListAsync();
        }

        public async Task<List<DisputeModel>> GetAsync(string reference)
        {
                   return await (from d in _context.Disputes.AsNoTracking()
                          from s in _context.Settlements.AsNoTracking().Where(s => s.Reference == d.Reference && s.Type != TransactionTypes.Charges).DefaultIfEmpty()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == d.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where d.Reference == reference
                          select new DisputeModel
                          {
                              CreatedAt = a.CreatedAt,
                              AccountName = a.Name,
                              AccountId = d.AccountId,
                              Status = d.Status,
                              Reference = d.Reference,
                              EmailAddress = u.EmailAddress,
                              Comment = d.Comment,
                              Type = d.Type,
                              Id = d.Id,
                              UpdatedAt = a.CreatedAt,
                          }).ToListAsync();
        }

        public async Task<bool> InsertAsync(Dispute dispute)
        {
            try
            {
                _context.Disputes.Add(dispute);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(DisputeRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Dispute dispute)
        {
            try
            {
                _context.Disputes.Update(dispute);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(DisputeRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
