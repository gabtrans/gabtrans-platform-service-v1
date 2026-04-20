using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitusClientRequest
    {
        public string name { get; set; }
        public string contactFirstName { get; set; }
        public string contactLastName { get; set; }
        public string contactEmail { get; set; }
        public string type { get; set; }
        public string onboardingRedirectUrl { get; set; }
        public bool useHostedFlow { get; set; }
    }
}
