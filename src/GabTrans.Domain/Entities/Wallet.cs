using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Wallet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public decimal Balance { get; set; }

    public string? Asset { get; set; }

    public string? Network { get; set; }

    public string? Uuid { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();
}
