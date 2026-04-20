using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GabTrans.Domain.Entities;

public partial class MessageCredential
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Host { get; set; }

    public string? Port { get; set; }

    public string? DisplayName { get; set; }

    public string? Encryption { get; set; }

    public string? Sender { get; set; }
}
