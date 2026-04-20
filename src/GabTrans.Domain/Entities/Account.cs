using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public string? Name { get; set; }

    public string Type { get; set; } = null!;

    public string? Uuid { get; set; }

    public string Status { get; set; } = null!;

    public string? Provider { get; set; }

    public string? Assets { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();

    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VirtualAccount> VirtualAccounts { get; set; } = new List<VirtualAccount>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
