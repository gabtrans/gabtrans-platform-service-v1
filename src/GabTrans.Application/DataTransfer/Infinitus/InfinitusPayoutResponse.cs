using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusPayoutResponse
    {
        public double amountRecipientReceives { get; set; }
        public double amountPayerPays { get; set; }
        public string shortReferenceId { get; set; }
        public string paymentCurrency { get; set; }
        public string sourceCurrency { get; set; }
        public double feeAmount { get; set; }
        public string feeCurrency { get; set; }
        public string recipientId { get; set; }
        public string paymentDate { get; set; }
        public string reference { get; set; }
        public string reason { get; set; }
        public string paymentMethod { get; set; }
        public string status { get; set; }
        public string paymentId { get; set; }
        public string? failureReason { get; set; }
        public string? failureType { get; set; }
        public InfinitusRecipientResponse recipient { get; set; }
    }
}
