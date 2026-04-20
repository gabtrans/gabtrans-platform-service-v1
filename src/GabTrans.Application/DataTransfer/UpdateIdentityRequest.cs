using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateIdentityRequest
    {
        public string IdentityNumber { get; set; }
        public string IdentityType { get; set; }
        public DateTime IdentityIssueDate { get; set; }
        public DateTime IdentityExpiryDate { get; set; }
        public string IdentityDocumentFront { get; set; }
        public string? IdentityDocumentBack { get; set; }
    }
}
