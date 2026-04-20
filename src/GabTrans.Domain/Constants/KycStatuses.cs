using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class KycStatuses
    {
        public const string Pending = "pending";
        public const string Rejected = "rejected";
        public const string Approved = "approved";
        public const string Submitted = "submitted";
        public const string Completed = "completed";
        public const string New = "new";
        public const string Passed = "passed";
        public const string Failed = "failed";
        public const string Error = "error";
    }
}
