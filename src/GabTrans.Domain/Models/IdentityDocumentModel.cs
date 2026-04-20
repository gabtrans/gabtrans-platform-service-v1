using System;

namespace GabTrans.Domain.Models;

public class IdentityDocumentModel
{
    public string IdentityNumber { get; set; }
    public string IdentityType { get; set; }
    public string IdentityIssueDate { get; set; }
    public string IdentityExpiryDate { get; set; }
    public string IdentityDocumentFront { get; set; }
    public string? IdentityDocumentBack { get; set; }
}
