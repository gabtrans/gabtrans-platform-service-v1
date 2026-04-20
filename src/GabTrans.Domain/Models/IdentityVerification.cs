using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class IdentityVerification
    {
        public string Usability { get; set; }
        public string DataChecks { get; set; }
        public string ImageChecks { get; set; }
        public string Extraction { get; set; }
        public string Similarity { get; set; }
        public string IdType { get; set; }
        public string DOB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentNo { get; set; }
        public string Expiry { get; set; }
        public string PersonalNumber { get; set; }
        public string MRZFormat { get; set; }
        public string IssuingCountry { get; set; }
    }
}
