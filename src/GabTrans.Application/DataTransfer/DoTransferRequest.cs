using GabTrans.Domain.Entities;
using System;

namespace GabTrans.Application.DataTransfer;

public class DoTransferRequest
{
    public Transfer  Transfer { get; set; }
    public TransferRecipient  TransferRecipient { get; set; }
}
