using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class AuthCredential
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string AppId { get; set; } = null!;

    public string AppKey { get; set; } = null!;

    public string? Token { get; set; }

    public string Status { get; set; } = null!;
}
