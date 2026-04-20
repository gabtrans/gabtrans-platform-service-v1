using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class DocumentInfinitusRequest
    {
        public byte[] file { get; set; }
        public string type { get; set; }
    }
}
