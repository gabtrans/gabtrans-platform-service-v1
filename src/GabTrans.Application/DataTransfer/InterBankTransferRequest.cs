using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class InterBankTransferRequest
    {
        public string Amount { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string BeneficiaryBankVerificationNumber { get; set; }
        public string KYCLevel { get; set; }
        public string sessionID { get; set; }
        public string BankCode { get; set; }
        public string BankVerification { get; set; }
        public string TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Narration { get; set; }
    }
}
