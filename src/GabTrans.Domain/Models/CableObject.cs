using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CableObject
    {
        public string TransId { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Package { get; set; }
        public decimal Amount { get; set; }
    }
}
