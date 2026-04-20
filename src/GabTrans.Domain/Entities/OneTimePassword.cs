using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class OneTimePassword
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[Column("id")]
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? UsedOn { get; set; }

    public bool Used { get; set; } 

    public long OtpCategoryId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual OtpCategory OtpCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
