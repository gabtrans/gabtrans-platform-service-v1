using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class EnergyReceipt
    {
        public string MeterType { get; set; }
        public string OrderNumber { get; set; }
        public string TransactionDate { get; set; }
        public string ReceiptNo { get; set; }
        public string TransactionReceiptNo { get; set; }
        public string ProductName { get; set; }
        public string PaymentType { get; set; }
        public string MeterNumber { get; set; }
        public string CurrentBalance { get; set; }
        public string SGC { get; set; }
        public string Address { get; set; }
        public string OutstandingDebtPaid { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Vat { get; set; }
        public string RemainingDebt { get; set; }
        public string AccountType { get; set; }
        public string OrgName { get; set; }
        public string OrgNumber { get; set; }
        public string Amount { get; set; }
        public string CostOfUnit { get; set; }
        public string FixedCharge { get; set; }
        public string TransactionStatus { get; set; }
        public string TariffDescription { get; set; }
        public string Units { get; set; }
        public string Rate { get; set; }
        public string Token { get; set; }
        public string ReferenceNumber { get; set; }
        public string Debt { get; set; }
        public string PhoneNumber { get; set; }
        public string TotalUnits { get; set; }
        public string Penalty { get; set; }
        public string LOR { get; set; }
        public string TransId { get; set; }
        public string Reconnection { get; set; }
        public string MeterService { get; set; }
        public string MeterReplacement { get; set; }
        public string InstallationFee { get; set; }
        public string AdministrativeCost { get; set; }
        public List<string> ChangeToken { get; set; } = new List<string>();
    }
}
