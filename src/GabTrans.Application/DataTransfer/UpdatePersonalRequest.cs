using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdatePersonalRequest
    {
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TaxNumber { get; set; }
        public string Citizenship { get; set; }
        public string TaxDocument { get; set; }
        public string BankStatement { get; set; }
    }
}
