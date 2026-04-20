using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class LimitDetails
    {
        public long Id { get; set; }
        public string Transfer { get; set; }
        public long TransferId { get; set; }
        public decimal Amount { get; set; }
        public long RoleId { get; set; }
        public string Role { get; set; } 
    }
}
