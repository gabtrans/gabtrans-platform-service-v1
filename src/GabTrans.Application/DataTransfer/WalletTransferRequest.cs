using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class WalletTransferRequest : BaseRequest
    {
        public string Amount { get; set; }
        [Display(Name = "Sender Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Beneficiary Account Number")]
        public string BeneficiaryAccountNumber { get; set; }
        [Display(Name = "Beneficiary Account Name")]
        public string BeneficiaryAccountName { get; set; }
        public string Narration { get; set; }
        public long WalletTypeId { get; set; }
        public bool AddAsBen { get; set; }
        [Display(Name = "Beneficiary")]
        public long? BeneficiaryId { get; set; }
        [Display(Name = "Bank")]
        public string BankCode { get; set; }
    }
}
