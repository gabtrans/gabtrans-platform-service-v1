using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GabTrans.Infrastructure.Repositories;

public class PlatformTransactionRepository(ILogService logService, GabTransContext dbContext) : IPlatformTransactionRepository
{
    private readonly ILogService _logService = logService;
    private readonly GabTransContext _dbContext = dbContext;

    public async Task<IEnumerable<PlatformTransaction>> GetAsync()
    {
        return await _dbContext.PlatformTransactions.ToListAsync();
    }

    public async Task<IEnumerable<PlatformTransaction>> GetAsync(long accountId)
    {
        return await _dbContext.PlatformTransactions.Where(x => x.AccountId == accountId && x.CreatedAt.Date == DateTime.UtcNow.Date).ToListAsync();
    }

    public async Task<IEnumerable<PlatformTransaction>> GetAsync(long accountId, string status)
    {
        return await _dbContext.PlatformTransactions.OrderBy(x => x.CreatedAt).Where(x => x.AccountId == accountId && x.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<PlatformTransaction>> GetAsync(string status, List<long> accountIds)
    {
        DateTime startDate = DateTime.Now.AddDays(-4);
        DateTime endDate = DateTime.Now;
        return await _dbContext.PlatformTransactions.Where(x => x.CreatedAt.Date >= startDate.Date && x.CreatedAt.Date <= endDate.Date).OrderBy(x => x.CreatedAt).Where(x => x.Status == status && accountIds.Contains(x.AccountId)).ToListAsync();
    }

    public async Task<IEnumerable<PlatformTransaction>> GetPendingAsync()
    {
        return await _dbContext.PlatformTransactions.OrderByDescending(x => x.Id).Where(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-10) && x.Status == null).Take(50).ToListAsync();
    }

    public async Task<PlatformTransaction> DetailsAsync(string reference)
    {
        return await _dbContext.PlatformTransactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
    }

    public async Task<PlatformTransaction> DetailsAsync(long id)
    {
        return await _dbContext.PlatformTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<PlatformTransaction>> GetPendingAsync(long accountId)
    {
        return await _dbContext.PlatformTransactions.Where(x => x.AccountId == accountId && (x.Status == GRemitStatuses.Ready)).ToListAsync();
    }

    public async Task<IEnumerable<GremitAccount>> GetGRemitAsync(string status)
    {
        return await _dbContext.GremitAccounts.Where(x => x.Status == status).ToListAsync();
    }

    public async Task<bool> UpdateAsync(string reference, string response)
    {
        try
        {
            var platformTransaction = await _dbContext.PlatformTransactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
            if (platformTransaction == null) return false;
            platformTransaction.Response = response;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(UpdateAsync), ex);
        }

        return false;
    }

    public async Task<bool> UpdateStatusAsync(long id, string status)
    {
        try
        {
            var remitlyTransaction = await _dbContext.PlatformTransactions.FindAsync(id);
            if (remitlyTransaction == null) return false;
            remitlyTransaction.Status = status;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(UpdateStatusAsync), ex);
        }

        return false;
    }

    public async Task<bool> CloseAsync(string reference, string status)
    {
        try
        {
            var remitlyTransaction = await _dbContext.PlatformTransactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
            if (remitlyTransaction == null) return false;
            remitlyTransaction.Status = status;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(CloseAsync), ex);
        }

        return false;
    }

    public async Task<bool> UpdateStatusAsync(string reference, string status, string? response = null)
    {
        try
        {
            var tranDetails = await _dbContext.PlatformTransactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
            if (tranDetails is null) return false;
            tranDetails.Status = status;
            if (response is not null) tranDetails.Response = response;
            //tranDetails.UpdatedAt=DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(UpdateStatusAsync), ex);
        }
        return false;
    }


    public async Task<bool> InsertAsync(long accountId, string reference, string request, string gateway)
    {
        try
        {
            var platformTransactions = await _dbContext.PlatformTransactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
            if (platformTransactions is not null && string.IsNullOrEmpty(platformTransactions.Response)) return true;
            if (platformTransactions is not null) return false;

            await _dbContext.PlatformTransactions.AddAsync(new PlatformTransaction
            {
                AccountId = accountId,
                Reference = reference,
                Gateway = gateway,
                Request = request,
                Status = GRemitStatuses.Ready,
                Response = string.Empty
            });

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(InsertAsync), ex);
        }

        return false;
    }


    public async Task<bool> BulkInsertAsync(IEnumerable<PlatformTransaction> gatewayPayouts)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            _dbContext.PlatformTransactions.AddRange(gatewayPayouts);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError(nameof(PlatformTransactionRepository), nameof(BulkInsertAsync), ex);
            await transaction.RollbackAsync();
        }

        return false;
    }

    //public async Task<bool> UpdateApplicationAsync(long id)
    //{
    //    try
    //    {
    //        var remitlyTransaction = await _dbContext.GremitApplications.FirstOrDefaultAsync(x => x.Id == id);
    //        if (remitlyTransaction == null) return false;
    //        if (remitlyTransaction.Active) remitlyTransaction.Active = false;
    //        if (!remitlyTransaction.Active) remitlyTransaction.Active = true;
    //        await _dbContext.SaveChangesAsync();
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logService.LogError(nameof(GatewayPayoutRepository), nameof(UpdateApplicationAsync), ex);
    //    }

    //    return false;
    //}
}
