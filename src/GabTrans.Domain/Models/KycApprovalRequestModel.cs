using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class KycApprovalRequestModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public long TotalTransaction { get; set; }
        public string Type { get; set; }
    }
}
