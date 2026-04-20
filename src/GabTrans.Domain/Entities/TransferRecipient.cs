using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class TransferRecipient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long AccountId { get; set; }

    public string Type { get; set; } = null!;

    public string? Name { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Country { get; set; }

    public string? PostCode { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? BankCountry { get; set; }

    public string? Currency { get; set; }

    public string? BankAccountType { get; set; }

    public string? SwiftCode { get; set; }

    public string? RoutingNumber { get; set; }

    public string? AccountNumber { get; set; }

    public string? AccountName { get; set; }

    public string? AccountRoutingType { get; set; }

    public string? BankBranch { get; set; }

    public string? BankName { get; set; }

    public string? BankStreetAddress { get; set; }

    public string? BankCity { get; set; }

    public string? BankPostalCode { get; set; }

    public string? BankState { get; set; }

    public string? IntermediaryBankCountry { get; set; }

    public string? IntermediaryPostalCode { get; set; }

    public string? IntermediaryCity { get; set; }

    public string? IntermediaryState { get; set; }

    public string? IntermediaryStreet1 { get; set; }

    public string? IntermediaryStreet2 { get; set; }

    public string? IntermediaryBankName { get; set; }

    public string? IntermediaryRoutingCode { get; set; }

    public string? InternationalBankName { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Uuid { get; set; }
}
