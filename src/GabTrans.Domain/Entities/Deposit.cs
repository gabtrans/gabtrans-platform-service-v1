using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Deposit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public string Reference { get; set; } = null!;

    public string? PayerAccountNumber { get; set; }

    public string? PayerAccountName { get; set; }

    public string? PayerBank { get; set; }

    public decimal Amount { get; set; }

    public decimal SettledAmount { get; set; }

    public decimal Fee { get; set; }

    public string Status { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string? Narration { get; set; }

    public string? GatewayResponse { get; set; }

    public string? Type { get; set; }

    public string? ResponseMessage { get; set; }

    public string? GatewayPostBack { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? GatewayReference { get; set; }

    public string? PayerCountry { get; set; }

    public bool Suspicious { get; set; }

    public virtual Account Account { get; set; } = null!;
}
