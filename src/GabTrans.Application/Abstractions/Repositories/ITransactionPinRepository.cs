using GabTrans.Domain.Entities;
using System;
using System.Transactions;


namespace GabTrans.Application.Abstractions.Repositories;

public interface ITransactionPinRepository
{
    Task<TransactionPin> GetAsync(long userId);
    Task<bool> InsertAsync(TransactionPin authorizationPin);
    Task<bool> UpdateAsync(TransactionPin authorizationPin);
}

