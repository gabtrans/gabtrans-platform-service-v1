using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BusinessObject
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string OperatingAddress { get; set; } = null!;

        public string? RegisteredAddress { get; set; }

        public DateTime? DateOfIncorporation { get; set; }

        public string? LicenseNumber { get; set; }

        public string? CertificateFront { get; set; }

        public string? CertificateBack { get; set; }

        public bool IsActive { get; set; }

        public long BusinessSectorId { get; set; }

        public long? RegistrationBodyId { get; set; }

        public string? Introduction { get; set; }

        public string? BusinessType { get; set; }

        public string? OperatingStreet { get; set; }

        public string? OperatingCity { get; set; }

        public string? PostalCode { get; set; }
    }
}
