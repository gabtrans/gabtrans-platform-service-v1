using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusVirtualAccountResponse
{
    public GlobusVirtualAccountResult Result { get; set; }
    public string Responsecode { get; set; }
    public string Responsemessage { get; set; }

    public class GlobusVirtualAccountResult
    {
        public string Msg { get; set; }
        public string VirtualAccount { get; set; }
    }
}
