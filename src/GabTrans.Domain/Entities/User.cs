using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public string? OldPassword { get; set; }

    public string Status { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LockedAt { get; set; }

    public string? LastLogin { get; set; }

    public string? DeviceId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual ICollection<Kyc> Kycs { get; set; } = new List<Kyc>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<OneTimePassword> OneTimePasswords { get; set; } = new List<OneTimePassword>();

    public virtual ICollection<TransactionPin> TransactionPins { get; set; } = new List<TransactionPin>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
