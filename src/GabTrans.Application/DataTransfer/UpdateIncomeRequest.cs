using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateIncomeRequest
    {
        public string IncomeSource { get; set; }
        public string IncomeState { get; set; }
        public string IncomeCountry { get; set; }
        public string SourceOfFunds { get; set; }
        public string WealthSource { get; set; }
        public string WealthSourceDescription { get; set; }
        public string AnnualIncome { get; set; }
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? OwnershipPercentage { get; set; }
        public bool IsSigner { get; set; }
    }
}
