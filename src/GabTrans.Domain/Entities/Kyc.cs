using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Kyc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? ResidentialState { get; set; }

    public string? TaxNumber { get; set; }

    public string? Citizenship { get; set; }

    public string? IdentityNumber { get; set; }

    public string? IdentityType { get; set; }

    public DateTime? IdentityIssueDate { get; set; }

    public DateTime? IdentityExpiryDate { get; set; }

    public string? PostalCode { get; set; }

    public DateTime? DateCompleted { get; set; }

    public DateTime? DateVerified { get; set; }

    public string? Selfie { get; set; }

    public string? Country { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public string? Occupation { get; set; }

    public string? IncomeRange { get; set; }

    public string? OccupationDescription { get; set; }

    public string? EmploymentStatus { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DateApproved { get; set; }

    public long? ApprovedBy { get; set; }

    public string? Industry { get; set; }

    public string? Employer { get; set; }

    public string? EmployerState { get; set; }

    public string? EmployerCountry { get; set; }

    public string? IncomeSource { get; set; }

    public string? IncomeState { get; set; }

    public string? IncomeCountry { get; set; }

    public string? WealthSource { get; set; }

    public string? WealthSourceDescription { get; set; }

    public string? AnnualIncome { get; set; }

    public string? BusinessDetails { get; set; }

    public bool OnboardedRepresentatives { get; set; }

    public string? ReasonForRejection { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? BankStatement { get; set; }

    public string? IdentityDocumentFront { get; set; }

    public string? TaxDocument { get; set; }

    public bool HasPin { get; set; }

    public bool VerifyEmail { get; set; }

    public string? Uuid { get; set; }

    public bool IsSigner { get; set; }

    public string? Title { get; set; }

    public string? OwnershipPercentage { get; set; }

    public string? Role { get; set; }

    public string? Type { get; set; }

    public bool UpdateEmployment { get; set; }

    public bool UpdateIdentity { get; set; }

    public bool UpdatePersonal { get; set; }

    public string? SourceOfFund { get; set; }

    public string? IdentityDocumentBack { get; set; }

    public string? ProofOfAddress { get; set; }

    public string? Gender { get; set; }

    public string? MaritalStatus { get; set; }

    public bool Verified { get; set; }

    public bool DataUploaded { get; set; }

    public bool DocumentUploaded { get; set; }

    public string Outcome { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
