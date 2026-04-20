using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class InfinitusTransactionStatuses
    {
        public const string Pending = "pending";
        public const string Settled = "settled";
        public const string Declined = "declined";
        public const string Completed = "completed";
        public const string AwaitingFunds = "awaiting_funds";
        public const string Reversed = "reversed";
        public const string Failed = "failed";
        public const string InReview = "in_review";
        public const string Processing = "processing";
        public const string Canceled = "canceled";
        public const string Accepted = "accepted";
        public const string Scheduled = "scheduled";
        public const string Sent = "sent";
        public const string ApprovalBlocked = "approval_blocked";
        public const string Returned = "returned";
        public const string Initiated = "initiated";
        public const string ApprovalRecalled = "approval_recalled";
        public const string ApprovalRejected = "approval_rejected";
        public const string CancellationRequested = "cancellation_requested";
        public const string InApproval = "in_approval";
        public const string Overdue = "overdue";
    }
}
