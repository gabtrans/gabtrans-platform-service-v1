using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusNameEnquiryResponse
{
    public GlobusNameEnquiryResult result { get; set; }
    public string responsecode { get; set; }
    public string responsemessage { get; set; }

    public class GlobusNameEnquiryResult
    {
        public string accountnumber { get; set; }
        public string accountname { get; set; }
        public string sessionid { get; set; }
        public string currencycode { get; set; }
    }
}
