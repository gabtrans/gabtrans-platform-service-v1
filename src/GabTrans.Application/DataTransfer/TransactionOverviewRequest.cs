using System;

namespace GabTrans.Application.DataTransfer;

public class TransactionCountRequest
{
    public decimal TotalTransaction { get; set; }
    public decimal FundTransfer { get; set; }
    public decimal CurrencyConversion { get; set; }
    public decimal CryptoTrade { get; set; }
}
