using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class AccountType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;
}
