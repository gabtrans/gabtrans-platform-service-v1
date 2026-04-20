using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusAccountRequestsResponse
    {
        public bool hasMore { get; set; }
        public List<InfinitusAccountRequestItem> items { get; set; }
    }

    public class InfinitusAccountRequestItem
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string accountId { get; set; }
        public string provider { get; set; }
        public DateTime createdAt { get; set; }
        public List<object> errors { get; set; }
        public List<InfinitusAccountRequestRepresentative> representatives { get; set; }
    }

    public class InfinitusAccountRequestRepresentative
    {
        public string id { get; set; }
        public string status { get; set; }
    }
}
