using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class AuditDetails
    {
        public string IPAddress { get; set; }
        public string Browser { get; set; }
        public string ModuleName { get; set; }
        public string UserAction { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
