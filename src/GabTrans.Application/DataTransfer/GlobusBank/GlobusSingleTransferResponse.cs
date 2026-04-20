using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusSingleTransferResponse
{
    public GlobusSingleTransferResult result { get; set; }
    public string responsecode { get; set; }
    public string responsemessage { get; set; }

    public class GlobusSingleTransferResult
    {
        public double amount { get; set; }
        public string balance { get; set; }
        public string transactionreference { get; set; }
        public object nipsession { get; set; }
        public string cbareference { get; set; }
        public object error { get; set; }
    }
}
