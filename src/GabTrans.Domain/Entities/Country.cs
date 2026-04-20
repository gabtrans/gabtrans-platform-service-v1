using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Code2 { get; set; } = null!;

    public string ContinentCode { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Flag { get; set; } = null!;
}
