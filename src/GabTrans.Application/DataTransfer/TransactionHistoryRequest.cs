using System;

namespace GabTrans.Application.DataTransfer;

public class TransactionHistoryRequest
{
    public long? Id { get; set; }
    public string? AccountName { get; set; }
    public string? Email { get; set; }
    public decimal? Amount { get; set; }
    public string? Status { get; set; }
    public string? TransactionType { get; set; }
    public string? Reference { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;
}
