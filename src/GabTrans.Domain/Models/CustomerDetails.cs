using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CustomerDetails
    {
        public CustomerInfoObject CustomerInfo { get; set; }
        public IndividualAccount IndividualAccount { get; set; }
        public List<BusinessAccountModel> BusinessAccounts { get; set; }
    }
}
