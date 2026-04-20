using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateRiskParameter : BaseRequest
    {
        public string SubsidiaryCode { get; set; }
        public long RiskFactorId { get; set; }
        public long WeightingFactor { get; set; }
        public long? AccountType { get; set; }
    }
}
