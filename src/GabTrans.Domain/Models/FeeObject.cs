using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FeeObject
    {
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal MaxAmount { get; set; }
     }
}
