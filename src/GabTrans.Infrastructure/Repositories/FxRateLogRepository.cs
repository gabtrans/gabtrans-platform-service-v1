using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class FxRateLogRepository(GabTransContext context, ILogService logService) : IFxRateLogRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<FxRateLog> GetAsync(string token)
        {
            return await _context.FxRateLogs.Where(x => x.RateToken == token).FirstOrDefaultAsync();
        }

        public async Task<FxRateLog> GetAsync(long id)
        {
            return await _context.FxRateLogs.FindAsync(id);
        }

        public async Task<bool> InsertAsync(FxRateLog fxRateLog)
        {
            try
            {
                _context.FxRateLogs.Add(fxRateLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxRateLogRepository), nameof(InsertAsync), ex);
            }
            return false;
        }

        public async Task<bool> UpdateAsync(FxRateLog fxRateLog)
        {
            try
            {
                _context.FxRateLogs.Update(fxRateLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(FxRateLogRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
