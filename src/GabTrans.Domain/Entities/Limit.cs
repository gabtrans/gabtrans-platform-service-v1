using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Limit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public decimal? SingleCumulative { get; set; }

    public decimal? DailyCumulative { get; set; }

    public long? DailyCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? TransactionType { get; set; }

    public string? AccountType { get; set; }

    public string Currency { get; set; } = null!;
}
