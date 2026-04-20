using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class PinObject
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string? OldPin { get; set; }

        public string NewPin { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
