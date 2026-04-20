using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class FxTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public string Reference { get; set; } = null!;

    public string FromCurrency { get; set; } = null!;

    public string ToCurrency { get; set; } = null!;

    public decimal ToAmount { get; set; }

    public decimal FromAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? ShortReferenceId { get; set; }

    public DateTime? ConversionDate { get; set; }

    public decimal CounterRate { get; set; }

    public string? Comment { get; set; }

    public string RateToken { get; set; } = null!;
}
