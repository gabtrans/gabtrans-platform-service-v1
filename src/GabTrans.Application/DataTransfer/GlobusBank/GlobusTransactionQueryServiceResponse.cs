using Newtonsoft.Json;
using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusTransactionQueryServiceResponse
{
    public string statusCode { get; set; }
    public string description { get; set; }
    public string sessionId { get; set; }
}
