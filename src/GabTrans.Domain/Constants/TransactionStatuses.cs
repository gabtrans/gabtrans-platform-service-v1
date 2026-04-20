using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class TransactionStatuses
    {
        public const string Pending = "pending";
        public const string Submitted = "submitted";
        public const string Processing = "processing";
        public const string Successful = "successful";
        public const string Success = "success";
        public const string Failed = "failed";
        public const string Approved = "approved";
        public const string Rejected = "rejected";
        public const string Reversed = "reversed";
        public const string Processed = "processed";
    }
}
