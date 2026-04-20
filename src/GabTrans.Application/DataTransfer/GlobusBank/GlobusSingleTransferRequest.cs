using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusSingleTransferRequest
{
    public string SourceAccount { get; set; }
    public string SourceAccountName { get; set; }
    public string SourceCcy { get; set; }
    public double Amount { get; set; }
    public string DestinationAccount { get; set; }
    public string DestinationAccountName { get; set; }
    public string DestinationCcy { get; set; }
    public string DestinationBankCode { get; set; }
    public string TransId { get; set; }
    public string AppUser { get; set; }
    public string Narration { get; set; }
    public string NipNameEnqRef { get; set; }
}
