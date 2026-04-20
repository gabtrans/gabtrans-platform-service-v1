using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Transfer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Reference { get; set; } = null!;

    public long AccountId { get; set; }

    public long TransferRecipientId { get; set; }

    public string SourceCurrency { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public string Status { get; set; } = null!;

    public string ProcessingStatus { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? GatewayRequest { get; set; }

    public string? GatewayResponse { get; set; }

    public string? QueryStatusResponse { get; set; }

    public string? FailureReason { get; set; }

    public string? PostBackResponse { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Gateway { get; set; } = null!;

    public string? GatewayReference { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string? Comment { get; set; }

    public virtual Account Account { get; set; } = null!;
}
