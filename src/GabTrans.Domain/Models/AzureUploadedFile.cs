using System;

namespace GabTrans.Domain.Models;

public class AzureUploadedFile
{
    public string? Uri { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public long? Size { get; set; }
}
