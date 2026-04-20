using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class SimulateInfinitusTopupRequest
    {
        public string amount { get; set; }
        public string globalAccountId { get; set; }
        public string counterPartyName { get; set; }
        public string reference { get; set; }
    }
}
