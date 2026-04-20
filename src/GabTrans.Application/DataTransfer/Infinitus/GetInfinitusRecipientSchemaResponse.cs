using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class GetInfinitusRecipientSchemaResponse
    {
        public List<InfinitusRecipientSchemaField> fields { get; set; }
        public List<string> supportedTransferMethods { get; set; }
    }

    public class InfinitusRecipientSchemaField
    {
        public string fieldName { get; set; }
        public string regex { get; set; }
        public string example { get; set; }
        //public string enum { get; set; }
        public bool required { get; set; }
    }
}
