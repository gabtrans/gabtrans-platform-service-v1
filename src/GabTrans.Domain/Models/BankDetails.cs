using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GabTrans.Domain.Models
{
    public class BankDetails
	{
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankState { get; set; }
        public string BankAddress { get; set; }
        public string BankName { get; set; }
        public string BicSwift { get; set; }
        public string Currency { get; set; }
    }
}

