using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CreateDisputeRequest
    {
        public string Reference { get; set; }
        public string? Comment { get; set; }
    }
}
