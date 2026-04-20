using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Permission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long ModuleActionId { get; set; }

    public virtual ApplicationModuleAction ModuleAction { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
