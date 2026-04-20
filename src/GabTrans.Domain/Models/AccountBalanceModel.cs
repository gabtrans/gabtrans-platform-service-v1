using System;

namespace GabTrans.Domain.Models;

public class AccountBalanceModel
{
    public long Id { get; set; }
    public string Assets { get; set; }
    public string Currency { get; set; }
    public string AccountName { get; set; }
    public decimal AvailableBalance { get; set; }
}
