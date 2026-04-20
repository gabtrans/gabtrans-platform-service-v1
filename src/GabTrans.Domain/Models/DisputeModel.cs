using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class DisputeModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string Reference { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string? Comment { get; set; }

        public string Status { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public string AccountName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
