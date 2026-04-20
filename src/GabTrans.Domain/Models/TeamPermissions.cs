using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TeamPermissions
    {
        public long? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public List<TeamPermissionsAction>? Actions { get; set; }
        public bool Checked { get; set; }
    }

    public class TeamPermissionsAction
    {
        public long? ModuleActionId { get; set; }
        public string? ModuleAction { get; set; }
    }
}
