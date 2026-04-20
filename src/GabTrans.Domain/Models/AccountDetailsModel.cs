using System;

namespace GabTrans.Domain.Models;

public class AccountDetailsModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? EmailAddress { get; set; }
    public string? Type { get; set; }
    public long UserId { get; set; }
    public string CreatedAt { get; set; }
    public PersonalInformationModel PersonalInformation { get; set; }
    public EmploymentInformationModel EmploymentInformation { get; set; }
    public IdentityDocumentModel IdentityDocument { get; set; }
    public BusinessAddressModel BusinessAddress { get; set; } = new BusinessAddressModel();
    public BusinessDocumentModel BusinessDocument { get; set; } = new BusinessDocumentModel();
    public BusinessInformationModel BusinessInformation { get; set; } = new BusinessInformationModel();
}

