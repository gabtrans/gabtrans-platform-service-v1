using System;
using Newtonsoft.Json;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualAccountMaxRequest
{
    public string AccountName { get; set; }

    public string LinkedPartnerAccountNumber { get; set; }
    // public string VirtualAccountNumber { get; set; }
    public bool CanExpire { get; set; }
    public int ExpiredTime { get; set; }
    public bool hasTransactionAmount { get; set; }
    public double TransactionAmount { get; set; }
    public string PartnerReference { get; set; }
}



