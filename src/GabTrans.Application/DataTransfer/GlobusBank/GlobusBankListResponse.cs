using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusBankListResponse
{
    public string responsecode { get; set; }
    public string responsemessage { get; set; }
    public List<GlobusBankListResult> result { get; set; }

    public class GlobusBankListResult
    {
        public string bank_name { get; set; }
        public string bank_code { get; set; }
    }
}
