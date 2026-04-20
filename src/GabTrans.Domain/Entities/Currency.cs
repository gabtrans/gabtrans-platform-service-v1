using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string? Symbol { get; set; }
}
