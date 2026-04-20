using System;

namespace GabTrans.Domain.Models;

public class TransactionModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string AccountName { get; set; }
    public string Email { get; set; }
    public string TransactionDate { get; set; }
    public string Reference { get; set; }
    public string TransactionType { get; set; }
    public string Amount { get; set; }
    public string Status { get; set; }
}
