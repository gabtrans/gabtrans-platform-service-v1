using System;
using System.Transactions;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly GabTransContext context;
    private readonly ILogService logService;

    public TransactionRepository(GabTransContext context, ILogService logService)
    {
        this.context = context;
        this.logService = logService;
    }

    public async Task<List<TransactionType>> GetTransactionTypeAsync()
    {
        return await context.TransactionTypes.OrderBy(x => x.Name).ToListAsync();
    }

    // public async Task<List<TransactionType>> GetTransactionOverviewAsync()
    // {
    //     return await context.TransactionTypes.OrderBy(x => x.Name).ToListAsync();
    // }

}
