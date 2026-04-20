using System;

namespace GabTrans.Application.DataTransfer;

public class QueryAccountFx
{
    public string FromCurrency { get; set; }
    public long AccountId { get; set; }
    public string ToCurrency { get; set; }
}
