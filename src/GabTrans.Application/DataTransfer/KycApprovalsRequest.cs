using System;

namespace GabTrans.Application.DataTransfer;

public class KycApprovalsRequest
{
    public long? Id { get; set; }
    public string? Status { get; set; }
    public string? EmailAddress { get; set; }
    public string? AccountName { get; set; }
    public string? Type { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;
}
