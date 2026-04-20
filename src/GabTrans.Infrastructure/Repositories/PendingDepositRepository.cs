using System;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories;

public class PendingDepositRepository(GabTransContext context, ILogService logService) : IPendingDepositRepository
{
    private readonly GabTransContext _context = context;
    private readonly ILogService _logService = logService;

    public async Task<PendingDeposit> DetailsAsync(long id)
    {
        return await _context.PendingDeposits.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<PendingDeposit> DetailsByReferenceAsync(string reference)
    {
        return await _context.PendingDeposits.Where(x => x.PaymentReference == reference).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PendingDeposit>> GetAsync()
    {
        return await _context.PendingDeposits.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<PendingDeposit>> GetAsync(string status)
    {
        return await _context.PendingDeposits.Where(x => x.Status == status).AsNoTracking().ToListAsync();
    }

    public async Task<bool> InsertAsync(PendingDeposit pendingDeposit)
    {
        try
        {
            _context.PendingDeposits.Add(pendingDeposit);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PendingDepositRepository), nameof(InsertAsync), ex);
        }
        return false;
    }

    public async Task<bool> UpdateAsync(PendingDeposit pendingDeposit)
    {
        try
        {
            _context.PendingDeposits.Update(pendingDeposit);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PendingDepositRepository), nameof(InsertAsync), ex);
        }
        return false;
    }
}
