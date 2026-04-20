using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class WebHook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Request { get; set; } = null!;

    public string Status { get; set; } = null!;

    public long? AccountId { get; set; }

    public string Provider { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string ReferenceNumber { get; set; } = null!;
}
