using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusBankAuthenticationResponse
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string token_type { get; set; }
    public string scope { get; set; }
}
