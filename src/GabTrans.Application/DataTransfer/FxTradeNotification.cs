using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FxTradeNotification
    {
        public string Status { get; set; }
        public string Reference { get; set; }
        public string EmailAddress { get; set; }
        public FxRateLog FxRateLog { get; set; }
    }
}
