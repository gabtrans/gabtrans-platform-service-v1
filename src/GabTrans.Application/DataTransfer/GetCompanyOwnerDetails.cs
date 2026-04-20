using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyOwnerDetails
    {
        public OfficerLinks links { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string occupation { get; set; }
        public Address address { get; set; }
        public string appointed_on { get; set; }
        public string country_of_residence { get; set; }
        public OfficerDateOfBirth date_of_birth { get; set; }
        public string officer_role { get; set; }
        public string etag { get; set; }
    }


    public class Officer
    {
        public string appointments { get; set; }
    }
}
