using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Entities
{
    public partial class PlatformTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string Reference { get; set; } = null!;

        public string Gateway { get; set; } = null!;

        public string Request { get; set; } = null!;

        public string? Response { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
