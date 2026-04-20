using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class SubmitInfinitusClientRequest
    {
        public List<string> providers { get; set; }
        public bool useHostedFlow { get; set; }
    }
}
