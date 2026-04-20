using System;

namespace GabTrans.Application.DataTransfer;

public class QueryAccountFee
{
    public string? Currency { get; set; }
    public long AccountId { get; set; }
    public string TransactionType { get; set; }
}
