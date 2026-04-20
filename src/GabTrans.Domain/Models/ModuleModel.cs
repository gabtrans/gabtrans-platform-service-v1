using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ModuleModel
    {
        public long? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public IEnumerable<Actions>? Actions { get; set; }
    }


    public class Actions
    {
        public long? ModuleActionId { get; set; }
        public string? ModuleActionName { get; set; }
        public bool Checked { get; set; }
    }
}
