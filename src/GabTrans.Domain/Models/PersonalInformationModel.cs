using System;

namespace GabTrans.Domain.Models;

public class PersonalInformationModel
{
    public string? PhoneNumber { get; set; }

    public string DateOfBirth { get; set; }

    public string? CitizenShip { get; set; }

    public string? BankStatement { get; set; }

    public string? TaxDocuments { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    // public string? Country { get; set; }

    public string? Status { get; set; }
}
