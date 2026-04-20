using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class WalletObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
