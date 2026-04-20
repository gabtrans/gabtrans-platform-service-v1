using System;

namespace GabTrans.Domain.Models;

public class GlobusSessionToken
{
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Expires { get; set; }
}
