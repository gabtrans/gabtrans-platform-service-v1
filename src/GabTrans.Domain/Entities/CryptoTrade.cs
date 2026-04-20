using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class CryptoTrade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public decimal FromAmount { get; set; }

    public string FromAsset { get; set; } = null!;

    public string FromNetwork { get; set; } = null!;

    public decimal? ToAmount { get; set; }

    public string ToAsset { get; set; } = null!;

    public string ToNetwork { get; set; } = null!;

    public string? Reference { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
