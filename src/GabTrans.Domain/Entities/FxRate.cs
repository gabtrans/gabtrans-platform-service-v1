using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class FxRate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string FromCurrency { get; set; } = null!;

    public string ToCurrency { get; set; } = null!;

    public decimal Rate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public decimal? FriendlyDisplayAmount { get; set; }

    public string? Type { get; set; }

    public long? UpdatedBy { get; set; }

    public decimal RateFromProvider { get; set; }

    public decimal RateMarkUp { get; set; }

    public long AccountId { get; set; }
}
