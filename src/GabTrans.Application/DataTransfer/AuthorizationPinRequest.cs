using System;

namespace GabTrans.Application.DataTransfer;

public class AuthorizationPinRequest
{
    public string Pin { get; set; }
    public string PinConfirmation { get; set; }
}
