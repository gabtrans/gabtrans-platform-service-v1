using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateEmploymentRequest
    {
        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string OccupationDescription { get; set; }
        public string Employer { get; set; }
        public string EmployerState { get; set; }
        public string EmployerCountry { get; set; }
        public string Industry { get; set; }
    }
}
