using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateBusinessDocumentRequest
    {
        public string FormationDocument { get; set; }
        public string ProofOfRegistration { get; set; }
        public string ProofOfOwnership { get; set; }
        public string BankStatement { get; set; }
        public string TaxDocument { get; set; }
        public string Agreement { get; set; }
    }
}
