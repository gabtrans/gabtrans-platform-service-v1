using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Business
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public string? Identifier { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public DateTime? IncorporationDate { get; set; }

    public string? TradeName { get; set; }

    public string? MonthlyRevenue { get; set; }

    public string? MonthlyConversionVolumeDigitalAssets { get; set; }

    public string? MonthlyLocalPaymentVolume { get; set; }

    public string? MonthlyConversionVolumeFiat { get; set; }

    public string? MonthlySwiftVolume { get; set; }

    public string? CurrencyNeeded { get; set; }

    public string? MainIndustry { get; set; }

    public string? Website { get; set; }

    public string? AdditionalIndustry { get; set; }

    public string? Naics { get; set; }

    public string? NaicsDescription { get; set; }

    public string? MailingAddress1 { get; set; }

    public string? MailingAddress2 { get; set; }

    public string? MailingCity { get; set; }

    public string? MailingPostalCode { get; set; }

    public string? MailingCountry { get; set; }

    public string? FormationDocument { get; set; }

    public string? ProofOfRegistration { get; set; }

    public string? ProofOfOwnership { get; set; }

    public string? BankStatement { get; set; }

    public string? TaxDocument { get; set; }

    public string? Uuid { get; set; }

    public string? CountriesOfOperation { get; set; }

    public string? Country { get; set; }

    public bool UpdateAddress { get; set; }

    public bool UpdateDocument { get; set; }

    public bool UpdateInformation { get; set; }

    public string? TaxId { get; set; }

    public string? Agreement { get; set; }

    public string? MailingState { get; set; }

    public bool DocumentUploaded { get; set; }

    public bool DataUploaded { get; set; }

    public virtual User User { get; set; } = null!;
}
