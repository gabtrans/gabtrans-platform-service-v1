using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Fee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string TransactionType { get; set; } = null!;

    public string MethodType { get; set; } = null!;

    public decimal Rate { get; set; }

    public string Currency { get; set; } = null!;

    public decimal CappedValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long AccountId { get; set; }
}
