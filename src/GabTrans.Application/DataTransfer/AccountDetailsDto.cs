using System;
using GabTrans.Domain.Models;

namespace GabTrans.Application.DataTransfer;

public class AccountDetailsMode
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? EmailAddress { get; set; }
    public string? Type { get; set; }
    public string CreatedAt { get; set; }

    public PersonalInformationModel PersonalInformationModels { get; set; }

    public EmploymentInformationModel EmploymentInformationModels { get; set; }

    public IdentityDocumentModel IdentityDocumentModels { get; set; }
}
