using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class PendingDeposit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string PaymentReference { get; set; } = null!;

    public string? SessionId { get; set; }

    public string BeneficiaryAccount { get; set; } = null!;

    public string Narration { get; set; } = null!;

    public string? AccountUuid { get; set; }

    public string? WalletUuid { get; set; }

    public double Amount { get; set; }

    public string Status { get; set; } = null!;

    public string Gateway { get; set; } = null!;

    public string? SenderAccountNumber { get; set; }

    public string? SenderAccountName { get; set; }

    public string? BankName { get; set; }

    public string? BankCode { get; set; }

    public string BeneficiaryAccountName { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string AllData { get; set; } = null!;

    public long RetryCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
