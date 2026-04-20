using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Settlement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long WalletId { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal PreviousBalance { get; set; }

    public decimal CurrentBalance { get; set; }

    public string Reference { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string DebitCreditIndicator { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Note { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
