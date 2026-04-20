using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GabTrans.Infrastructure.Repositories;

public class TransactionPinRepository(GabTransContext context, ILogService logService) : ITransactionPinRepository
{
    private readonly GabTransContext _context = context;
    private readonly ILogService _logService = logService;

    public async Task<TransactionPin> GetAsync(long userId)
    {
        return await _context.TransactionPins.Where(tp => tp.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> InsertAsync(TransactionPin authorizationPin)
    {
        try
        {
            _context.TransactionPins.Add(authorizationPin);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError("TransactionPinRepository", "InsertPinAsync", ex);
        }
        return false;
    }

    public async Task<bool> UpdateAsync(TransactionPin authorizationPin)
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogError("TransactionPinRepository", "UpdateAsync", ex);
        }
        return false;
    }


}
