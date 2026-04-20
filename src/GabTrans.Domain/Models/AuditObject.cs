using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class AuditObject
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ModuleActionId { get; set; }

        public long ChannelId { get; set; }

        public string? IpAddress { get; set; }

        public string? Browser { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
