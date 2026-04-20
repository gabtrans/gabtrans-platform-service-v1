using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using GabTrans.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories
{
    public class AuditRepository(GabTransContext context, ILogService logService) : IAuditRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> UpdateAsync(Audit audit)
        {
            try
            {
                _context.Audits.Update(audit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AuditRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<LoginObject> GetSessionDetailsAsync(long userId)
        {
            return await _context.Logins.Where(x => x.UserId == userId).Select(a => new LoginObject { Id = a.Id, Attempts = a.Attempts, SessionToken = a.SessionToken, UserId = a.UserId }).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Audit audit)
        {
            try
            {
                _context.Audits.Add(audit);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AuditRepository), nameof(InsertAsync), ex);
            }
        }

        public async Task InsertAsync(long actorId, long activity, long channelId, string browser, string? ipAddress = null, string? description = null)
        {
            try
            {
                var audit = new Audit
                {
                    CreatedAt = DateTime.Now,
                    ModuleActionId = activity,
                    ChannelId = channelId,
                    UserId = actorId,
                    Browser = browser,
                    IpAddress = ipAddress,
                    Description = description
                };

                _context.Audits.Add(audit);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AuditRepository), nameof(InsertAsync), ex);
            }
        }

        public async Task<List<AuditDetails>> GetAsync(GetAuditRequest request)
        {
            DateTime startDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(request.StartDate);
            DateTime endDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now : Convert.ToDateTime(request.EndDate);

            return await (from a in _context.Audits.AsNoTracking()
                          from ma in _context.ModuleActions.AsNoTracking().Where(ma => ma.Id == a.ModuleActionId).DefaultIfEmpty()
                          from m in _context.Modules.AsNoTracking().Where(m => m.Id == ma.ModuleId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where a.CreatedAt.Date >= startDate.Date && a.CreatedAt.Date <= endDate.Date
                          && (string.IsNullOrEmpty(request.Email) || u.EmailAddress == request.Email)
                          && (request.ModuleId == 0 || request.ModuleId == null || ma.ModuleId == request.ModuleId)
                          && (request.UserId == 0 || request.UserId == null || a.UserId == request.UserId)
                          select new AuditDetails
                          {
                              CreatedOn = a.CreatedAt,
                              FullName = $"{u.FirstName} {u.LastName}",
                              IPAddress = a.IpAddress,
                              ModuleName = m.Name,
                              UserAction = ma.UserAction,
                              Browser = a.Browser
                          }).OrderByDescending(x=>x.CreatedOn).ToListAsync();
        }

        public async Task<List<AuditDetails>> GetAsync(long userId)
        {
            return await (from a in _context.Audits.AsNoTracking()
                          from ma in _context.ModuleActions.AsNoTracking().Where(ma => ma.Id == a.ModuleActionId).DefaultIfEmpty()
                          from m in _context.Modules.AsNoTracking().Where(m => m.Id == ma.ModuleId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where 
                           a.UserId == userId
                          select new AuditDetails
                          {
                              CreatedOn = a.CreatedAt,
                              FullName = $"{u.FirstName} {u.LastName}",
                              IPAddress = a.IpAddress,
                              ModuleName = m.Name,
                              UserAction = ma.UserAction,
                              Browser = a.Browser
                          }).OrderByDescending(x => x.CreatedOn).Take(10).ToListAsync();
        }
    }
}

