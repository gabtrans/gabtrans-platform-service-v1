using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class VirtualAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public string? Type { get; set; }

    public string? ReferenceCode { get; set; }

    public string? BankStreet1 { get; set; }

    public string? BankStreet2 { get; set; }

    public string? BankCity { get; set; }

    public string? BankState { get; set; }

    public string? BankPostalCode { get; set; }

    public string? SwiftCode { get; set; }

    public string? RoutingNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
