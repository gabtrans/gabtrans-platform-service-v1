using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
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
    public class FxRateAuditRepository(GabTransContext context, ILogService logService) : IFxRateAuditRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService logService = logService;

        public async Task<IEnumerable<FxRateAudit>> GetAsync(QueryFxRate queryFxRate)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryFxRate.StartDate) ? DateTime.Now.AddDays(-7) : Convert.ToDateTime(queryFxRate.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryFxRate.EndDate) ? DateTime.Now : Convert.ToDateTime(queryFxRate.EndDate);

            return await (from a in _context.FxRateAudits.AsNoTracking()
                          where
                            (string.IsNullOrEmpty(queryFxRate.Currency) || a.FromCurrency == queryFxRate.Currency || a.ToCurrency == queryFxRate.Currency)
                              && a.CreatedAt.Date >= fromDate.Date && a.CreatedAt.Date <= toDate.Date
                          select a).ToListAsync();
        }


        public async Task<bool> InsertAsync(FxRateAudit fxRateAudit)
        {
            try
            {
                _context.FxRateAudits.Add(fxRateAudit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logService.LogError(nameof(FxRateAuditRepository), nameof(InsertAsync), ex);
            }

            return false;
        }
    }
}
