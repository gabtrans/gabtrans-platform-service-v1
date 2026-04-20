using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class AmlInfoObject
    {
        public CustomerInfoObject CustomerInfoObject { get; set; }
        public List<RiskInfoObject> RiskInfoObjects { get; set; }
    }
}
