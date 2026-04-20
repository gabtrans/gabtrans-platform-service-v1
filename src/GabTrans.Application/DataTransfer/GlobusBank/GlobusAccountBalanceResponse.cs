using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusAccountBalanceResponse
{
    public GlobusAccountBalanceResult result { get; set; }
    public string responsecode { get; set; }
    public string responsemessage { get; set; }

    public class GlobusAccountBalanceResult
    {
        public string accountNo { get; set; }
        public string accTitle { get; set; }
        public string currencyCode { get; set; }
        public double balance { get; set; }
    }
}
