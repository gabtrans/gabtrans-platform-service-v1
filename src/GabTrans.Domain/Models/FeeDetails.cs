using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FeeDetails
    {
        public long Id { get; set; }
        public string Transfer { get; set; }
        public double Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public long TransferId { get; set; }
    }
}
