using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualAccountMaxResponse
{
    public GlobusVirtualAccountMaxResult result { get; set; }
    public string responsecode { get; set; }
    public string responsemessage { get; set; }

    public class GlobusVirtualAccountMaxResult
    {
        public string msg { get; set; }
        public string virtualAccount { get; set; }
    }
}
