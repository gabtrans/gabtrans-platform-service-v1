using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class UserObject
    {
        public UserDetails User{ get; set; }
        public List<AuditDetails> Audits { get; set; }
        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}
