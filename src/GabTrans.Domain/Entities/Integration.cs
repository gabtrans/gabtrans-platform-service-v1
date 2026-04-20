using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Integration
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? UserName { get; set; }

    public string? SecretKey { get; set; }

    public string? CallBackUrl { get; set; }

    public string? PostBackUrl { get; set; }

    public string? PublicKey { get; set; }

    public string? Password { get; set; }
}
