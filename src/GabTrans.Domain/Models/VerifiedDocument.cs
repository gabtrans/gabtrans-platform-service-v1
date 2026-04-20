using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class VerifiedDocument
    {
        public string DocFront { get; set; }
        public string DocBack { get; set; }
        public string Face { get; set; }
        public string Liveness { get; set; }
    }
}
