using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualLiteResponse
{
    public GlobusVirtualLiteResult result { get; set; }
    public string responsecode { get; set; }
    public string responsemessage { get; set; }

    public class GlobusVirtualLiteResult
    {
        public string msg { get; set; }
        public string virtualAccount { get; set; }
    }
}
