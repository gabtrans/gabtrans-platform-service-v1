using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ProcessTransfer
    {
        public Transfer Transfer { get; set; }
        public TransferRecipient  TransferRecipient { get; set; }
    }
}
