using System;

namespace GabTrans.Domain.Models;

public class EmploymentInformationModel
{
    public string IncomeSource { get; set; }
    public string IncomeState { get; set; }
    public string IncomeCountry { get; set; }
    public string SourceOfFunds { get; set; }
    public string WealthSource { get; set; }
    public string WealthSourceDescription { get; set; }
    public string AnnualIncome { get; set; }
    public string? Role { get; set; }
    public string? Title { get; set; }
    public string? OwnershipPercentage { get; set; }
    public string EmploymentStatus { get; set; }
    public string Occupation { get; set; }
    public string OccupationDescription { get; set; }
    public string Employer { get; set; }
    public string EmployerState { get; set; }
    public string EmployerCountry { get; set; }
    public string Industry { get; set; }
}
