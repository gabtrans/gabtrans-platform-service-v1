using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusAccountRequestResponse
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string accountId { get; set; }
        public string provider { get; set; }
        public DateTime createdAt { get; set; }
        public List<object> errors { get; set; }
        public List<InfinitusAccountRequestRepresentative> representatives { get; set; }

    }
}
