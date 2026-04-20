using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualAccountFullRequest
{
    public string AccountName { get; set; }
    public string LinkedPartnerAccountNumber { get; set; }
    public string VirtualAccountNumber { get; set; }
    public string DOB { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Sex { get; set; }
    public string Maritalstatus { get; set; }
    public string CanExpire { get; set; }
    public int ExpiredTime { get; set; }
    public string PhoneNo { get; set; }
    public string CustomerAddress { get; set; }
    public bool IsCardable { get; set; }
    public string Email { get; set; }
    public bool hasTransactionAmount { get; set; }
    public int TransactionAmount { get; set; }
}
