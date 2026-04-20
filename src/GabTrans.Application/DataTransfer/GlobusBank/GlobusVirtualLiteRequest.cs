using System;
using Newtonsoft.Json;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualAccountLiteRequest
{
    public string AccountName { get; set; }

    [JsonProperty(" LinkedPartnerAccountNumber")]
    public string LinkedPartnerAccountNumber { get; set; }
    public string VirtualAccountNumber { get; set; }
    public string CanExpire { get; set; }
    public int ExpiredTime { get; set; }
    public bool hasTransactionAmount { get; set; }
    public int TransactionAmount { get; set; }

}

