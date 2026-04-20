using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SetupInfo
    {
        public string UniqueKey { get; set; }
        public string BarCode { get; set; }
        public string Pin { get; set; }
        public long UserId { get; set; }
    }
}
