using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class PermissionModel
    {
        public long Id { get; set; }
        public string Role { get; set; }
        public long RoleId { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public IEnumerable<PermissionActions>? PermissionActions { get; set; }
    }
}
