using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class BusinessOnboardingRequest : BaseRequest
    {
        public long AccountTypeId { get; set; }
        public string BusinessName { get; set; }
        public string RegisteredAddress { get; set; }
        public string OperatingAddress { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public long NatureOfBusinessId { get; set; }
        public long RegistrationBodyId { get; set; }
        public string LicenseIDNumber { get; set; }
        public long TransactionValue { get; set; }
        public long TransactionVolume { get; set; }
        public long FrequencyId { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeAddress { get; set; }
        public string RepresentativeDOB { get; set; }
        public long RepresentativeIdType { get; set; }
        //public string RepresentativeFile { get; set; }
        //public string RepresentativeFile2 { get; set; }
        //public string RepresentativeSelfie { get; set; }
    }
}
