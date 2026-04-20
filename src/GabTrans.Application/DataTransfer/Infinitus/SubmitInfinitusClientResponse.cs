using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class SubmitInfinitusClientResponse
    {
        public List<SubmitInfinitusClient> SubmitInfinitusClients { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
