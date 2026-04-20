using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class Audit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ModuleActionId { get; set; }

    public long ChannelId { get; set; }

    public string? IpAddress { get; set; }

    public string? Browser { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Description { get; set; }

    public DateTime UpdatedAt { get; set; }
}
