using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class EmailTemplates
    {
        public const string ForgotPassword = "forgot-password";
        public const string ReceiveMoney = "Receive_Money";
        public const string FundAccount = "fund-account";
        public const string AccountRequest = "account-request";
        public const string TransferRequest = "transfer-request";
        public const string AccountOpening = "account-opening";
        public const string AccountRejected = "account-rejected";
        public const string AccountApproved = "account-approved";
        public const string TransferRejected = "transfer-rejected";
        public const string TransferApproved = "transfer-approved";
        public const string ResetPassword = "reset-password";
        public const string Trading = "fx-conversion";
        public const string Otp = "otp";
        public const string EmailVerification = "email-verification";
        public const string FailedTransfer = "failed-transfer";
        public const string SuccessfulTransfer = "successful-transfer";
        public const string UserInvite = "user-invite";
        public const string NewDispute = "new-dispute";
        public const string UpdateDispute = "update-dispute";
        public const string LowBalance = "low-balance";
    }
}
