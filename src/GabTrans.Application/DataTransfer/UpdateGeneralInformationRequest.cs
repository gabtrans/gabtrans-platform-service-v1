using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateGeneralInformationRequest
    {
        public string? MainIndustry { get; set; }
        //public string? AdditionalIndustry { get; set; }
        // public string? ContactFirstName { get; set; }
        // public string? ContactLastName { get; set; }
        // public string? ContactEmail { get; set; }
        // public string? ContactPhone { get; set; }
        public string? NAICS { get; set; }
        public string? NAICSDescription { get; set; }
    }
}
