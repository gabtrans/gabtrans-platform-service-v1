using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CreatePermissionRequest
    {
        public long RoleId { get; set; }
        public List<long> ModuleActionIds { get; set; }
    }
}
