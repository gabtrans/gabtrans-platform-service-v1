using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusWebHook
{
    public int id { get; set; }
    public double actualAmount { get; set; }
    public object expectedAmount { get; set; }
    public string narration { get; set; }
    public string transactionType { get; set; }
    public string transactionRef { get; set; }
    public double availableBalance { get; set; }
    public string transactionDate { get; set; }
    public string nubanAccount { get; set; }
    public string transId { get; set; }
    public string destinationBankCode { get; set; }
    public string nubanAccountName { get; set; }
    public string virtualAccount { get; set; }
    public string virtualAccountName { get; set; }
    public string sourceBankCode { get; set; }
    public string sourceAccount { get; set; }
    public string sourceBankName { get; set; }
    public string sourceAccountName { get; set; }
    public string sessionId { get; set; }
    public string paymentStatus { get; set; }
    public string transactionStatus { get; set; }
    public int retries { get; set; }
}
