using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CompanyDetails
    {
        public string Name { get; set; }
        public string CompanyNumber { get; set; }
        public string CreationDate { get; set; }
        public string CompanyType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Locality { get; set; }
        public string PostalCode { get; set; }
    }
}
