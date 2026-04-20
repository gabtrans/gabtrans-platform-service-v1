using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class ApplicationModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Active { get; set; } = null!;

    public virtual ICollection<ApplicationModuleAction> ModuleActions { get; set; } = new List<ApplicationModuleAction>();
}
