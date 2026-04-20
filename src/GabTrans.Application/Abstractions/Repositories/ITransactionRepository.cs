using System;
using System.Transactions;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<List<TransactionType>> GetTransactionTypeAsync();
}
