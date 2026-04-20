using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Notification
{
    public interface IEmailNotificationService
    {
        Task<ApiResponse> SendMailAsync(SendMailRequest request);
        Task<bool> KycOutcomeAsync(string emailAddress, string firstName, string status, string comment, string templateId);
        Task<bool> FundAccountAsync(string senderName, string emailAddress, string currency, decimal amount, string referenceNumber, string templateId);
        Task<bool> SendMoneyAsync(SendMoneyReceipt sendMoney);
        Task<bool> ReceiveMoneyAsync(string receiverName, string emailAddress, string accountNumber, string accountName, string routingCode, string bank, string bankCountry, string currency, decimal amount, long paymentTypeId, string templateId);
        Task<bool> ResetPasswordAsync(string emailAddress, string templateId);
        Task<bool> ForgotPasswordAsync(string firstName,string emailAddress, string otp);
        Task<bool> OnboardingAsync(string customerName, string emailAddress, string templateId);
        Task<bool> AuthorizationAsync(string firstName, string emailAddress, string otp);
        Task<bool> EmailVerificationAsync(string emailAddress, string otp);
        Task<bool> SendVerificationLinkAsync(string firstName, string emailAddress, string link, string templateId);
        Task<bool> FailedVerificationAsync(string firstName, string lastName, string dojahReference, string templateId);
        Task<bool> InvitationAsync(string emailAddress, string token, string templateId, string inviterFullName, string inviteeDepartment, string emailSubject);
        Task<bool> AccountRequestAsync(string accountName, string accountType, DateTime createdAt);
        Task<bool> AccountOpeningAsync(string emailAddress, string firstName, string currency);
        Task<bool> AccountApprovedAsync(string emailAddress, string firstName);
        Task<bool> AccountRejectedAsync(string emailAddress, string firstName);
        Task<bool> FailedTransferAsync(Transfer transfer, string emailAddress, string accountNumber, string accountName, string bank);
        Task<bool> FundingAsync(string emailAddress, string currency, decimal amount);
        Task<bool> TransferRequestAsync(string accountName, string currency, decimal amount, DateTime transactionDate);
        Task<bool> SuccessfulTransferAsync(Transfer transfer, string emailAddress, string accountNumber, string accountName, string bank);
        Task<bool> FxTradeAsync(FxTradeNotification fxTradeNotification);
        Task<bool> ApprovedTransferAsync(string emailAddress, string firstName);
        Task<bool> RejectedTransferAsync(string emailAddress, string firstName);
        Task<bool> NewDisputeAsync(string accountName, string reference, string type, string currency, decimal amount, string comment, DateTime dateCreated);
        Task<bool> UpdateDisputeAsync(string email, string reference, string type, string currency, decimal amount, string comment, DateTime dateCreated);
    }
}
