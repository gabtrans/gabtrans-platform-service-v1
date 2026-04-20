using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SaveOfficers
    {
        public long UserId { get; set; }
        public long BusinessId { get; set; }
        public List<Officers> Officers { get; set; }
    }
}
