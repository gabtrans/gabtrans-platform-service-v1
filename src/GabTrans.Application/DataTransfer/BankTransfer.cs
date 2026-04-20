using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class BankTransfer
    {
        public decimal Fee { get; set; }
        public Account Account { get; set; }
        public Kyc Kyc { get; set; }
        public Wallet Wallet { get; set; }
        public BankTransferRequest BankTransferRequest { get; set; }
    }
}
