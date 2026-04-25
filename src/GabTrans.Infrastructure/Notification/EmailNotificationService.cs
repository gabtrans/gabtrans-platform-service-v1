using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System.Net.Mail;

namespace GabTrans.Infrastructure.Notification
{
    public class EmailNotificationService(ILogService logService, IFileService fileService, IReceiptService receiptService, IValidationService validationService, IMailClientIntegration mailClientIntegration) : IEmailNotificationService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly IReceiptService _receiptService = receiptService;
        private readonly IValidationService _validationService = validationService;
        private readonly IMailClientIntegration _mailClientIntegration = mailClientIntegration;

        public async Task<ApiResponse> SendMailAsync(SendMailRequest request)
        {
            string? copy = null;
            var result = new ApiResponse();
            try
            {
                // Console.WriteLine("Payload for payout notification:: " + JsonConvert.SerializeObject(request));
                bool send = await _mailClientIntegration.SendAsync(request);

                if (!send)
                {
                    return new ApiResponse
                    {
                        Message = "Unable to process request"
                    };
                }

                result.Success = true;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "SendMail", ex);
                result.Message = "Kindly try later";
            }
            return result;
        }

        public async Task<bool> ForgotPasswordAsync(string firstName, string emailAddress, string otp)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.ForgotPassword, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{OTP}", otp).Replace("{FirstName}", firstName).Replace("{ValidationTime}", StaticData.OtpLifetime.ToString());

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Authorization, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "ForgotPassword", ex);
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(string emailAddress, string templateId)
        {
            try
            {
                string content = _fileService.GetTemplate(templateId, Templates.Email);

                if (!string.IsNullOrEmpty(content))
                {
                    string subject = templateId.Replace("_", " ");

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = subject, Message = content });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "ResetPassword", ex);
            }
            return false;
        }


        public async Task<bool> KycOutcomeAsync(string emailAddress, string firstName, string status, string comment, string templateId)
        {
            try
            {
                string content = _fileService.GetTemplate(templateId, Templates.Email);

                if (!string.IsNullOrEmpty(content))
                {
                    string subject = templateId.Replace("_", " ");

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = subject, Message = content });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "KycOutcomeAsync", ex);
            }
            return false;
        }

        public async Task<bool> SendMoneyAsync(SendMoneyReceipt sendMoney)
        {
            try
            {
                string template = _fileService.GetTemplate(sendMoney.TemplateId, Templates.Email);

                string subject = sendMoney.TemplateId.Replace("_", " ");

                string content = template.Replace("{CUSTOMERNAME}", sendMoney.SenderFirstName).Replace("{AccountNumber}", sendMoney.AccountNumber ?? "N/A").Replace("{Currency}", sendMoney.Currency).Replace("{Amount}", sendMoney.Amount).Replace("{Date}", sendMoney.TransactionDate).Replace("{PaymentDays}", sendMoney.PaymentDays).Replace("{Description}", sendMoney.Description ?? "N/A").Replace("{AccountName}", sendMoney.BeneficiaryName).Replace("{Bank}", sendMoney.Bank).Replace("{PaymentType}", sendMoney.PaymentType).Replace("{SortCode}", sendMoney.SortCode ?? "N/A").Replace("{ABA}", sendMoney.ABA ?? "N/A").Replace("{SwiftCode}", sendMoney.SwiftCode ?? "N/A").Replace("{IBAN}", sendMoney.IBAN ?? "N/A").Replace("{BSBCode}", sendMoney.BSBCode ?? "N/A").Replace("{InstitutionNo}", sendMoney.InstitutionNo ?? "N/A").Replace("{BankCode}", sendMoney.BankCode ?? "N/A").Replace("{BranchCode}", sendMoney.BranchCode ?? "N/A").Replace("{ReferenceNumber}", sendMoney.ReferenceNumber ?? "N/A").Replace("{ShortReference}", sendMoney.ShortReference);

                return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = sendMoney.SenderEmailAddress, Subject = subject, Message = content });
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "SendMoney", ex);
            }
            return false;
        }

        public async Task<bool> ReceiveMoneyAsync(string receiverName, string emailAddress, string accountNumber, string accountName, string routingCode, string bank, string bankCountry, string currency, decimal amount, long paymentTypeId, string templateId)
        {
            try
            {
                string template = _fileService.GetTemplate(templateId, Templates.Email);

                string subject = templateId.Replace("_", " ");

                string content = template.Replace("{ReceiverName}", receiverName).Replace("{AccountNumber}", accountNumber).Replace("{Amount}", string.Concat(currency, amount.ToString("N2"))).Replace("{AccountName}", accountName).Replace("{Bank}", bank).Replace("{RoutingCode}", routingCode).Replace("{BankCountry}", bankCountry).Replace("{{Routing Code}}", paymentTypeId == PaymentTypes.International ? "Swift Code" : "Routing Code").Replace("{{Account Number}}", paymentTypeId == PaymentTypes.International ? "IBAN Number" : "Account Number");

                return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = subject, Message = content });
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "ReceiveMoney", ex);
            }
            return false;
        }



        public async Task<bool> FundAccountAsync(string senderName, string emailAddress, string currency, decimal amount, string referenceNumber, string templateId)
        {
            try
            {
                string template = _fileService.GetTemplate(templateId, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string subject = templateId.Replace("_", " ");

                    string emailContent = template.Replace("{ReferenceNumber}", referenceNumber).Replace("{SenderName}", senderName).Replace("{Last4WalletNumber}", "XXXX").Replace("{Amount}", amount.ToString("N2")).Replace("{Currency}", currency).Replace("{ValueDate}", DateTime.Now.ToString("dd MMM, yyyy"));

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.FundAccount, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "FundAccount", ex);
            }
            return false;
        }


        public async Task<bool> AuthorizationAsync(string firstName, string emailAddress, string otp)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.Otp, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{OTP}", otp).Replace("{FirstName}", firstName).Replace("{ValidationTime}", StaticData.OtpLifetime.ToString());

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Authorization, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> EmailVerificationAsync(string emailAddress, string otp)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.EmailVerification, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{OTP}", otp).Replace("{ValidationTime}", StaticData.OtpLifetime.ToString());

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.EmailVerification, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }


        public async Task<bool> AccountRequestAsync(string accountName, string accountType, DateTime createdAt)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.AccountRequest, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", "Team").Replace("{AccountName}", accountName).Replace("{AccountType}", accountType).Replace("{URL}", StaticData.AppUrl).Replace("{DateCreated}", createdAt.ToString("dd-MM-yyyy hh:mm:ss"));

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = StaticData.BackendEmailAddress, Subject = EmailSubjects.AccountRequest, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> AccountOpeningAsync(string emailAddress, string firstName, string currency)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.AccountOpening, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", firstName).Replace("{Currency}", currency).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.AccountOpening, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> AccountApprovedAsync(string emailAddress, string firstName)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.AccountApproved, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", firstName).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.AccountApproved, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> AccountRejectedAsync(string emailAddress, string firstName)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.AccountRejected, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", firstName).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.AccountRejected, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> FailedTransferAsync(Transfer transfer, string emailAddress, string accountNumber, string accountName, string bank)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.FailedTransfer, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{AccountName}", accountName).Replace("{Currency}", transfer.Currency).Replace("{Amount}", transfer.Amount.ToString("N2")).Replace("{Date}", transfer.CreatedAt.ToString("MMM dd, yyyy")).Replace("{ReferenceNumber}", transfer.Reference).Replace("{AccountNumber}", accountNumber).Replace("{Bank}", bank).Replace("{Description}", transfer.Reason);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Transfer, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Authorization", ex);
            }
            return false;
        }

        public async Task<bool> FundingAsync(string emailAddress, string currency, decimal amount)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.FundAccount, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{Currency}", currency).Replace("{Amount}", amount.ToString("N2")).Replace("{Url}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.FundAccount, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "FundingAsync", ex);
            }
            return false;
        }

        public async Task<bool> TransferRequestAsync(string accountName, string currency, decimal amount, DateTime transactionDate)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.TransferRequest, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{AccountName}", accountName).Replace("{Currency}", currency).Replace("{Amount}", amount.ToString("N2")).Replace("{Date}", transactionDate.ToString("MMM dd, yyyy")).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = StaticData.BackendEmailAddress, Subject = EmailSubjects.TransferRequest, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "PayoutRequestAsync", ex);
            }
            return false;
        }

        public async Task<bool> SuccessfulTransferAsync(Transfer transfer, string emailAddress, string accountNumber, string accountName, string bank)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.SuccessfulTransfer, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{AccountName}", accountName).Replace("{Currency}", transfer.Currency).Replace("{Amount}", transfer.Amount.ToString("N2")).Replace("{Date}", transfer.CreatedAt.ToString("MMM dd, yyyy")).Replace("{ReferenceNumber}", transfer.Reference).Replace("{AccountNumber}", accountNumber).Replace("{Bank}", bank).Replace("{Description}", transfer.Reason);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Transfer, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "SuccessfulPayoutAsync", ex);
            }
            return false;
        }

        public async Task<bool> FxTradeAsync(FxTradeNotification fxTradeNotification)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.Trading, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FromCurrency}", fxTradeNotification.FxRateLog.FromCurrency).Replace("{FromAmount}", fxTradeNotification.FxRateLog.FromAmount.ToString("N2")).Replace("{ToCurrency}", fxTradeNotification.FxRateLog.ToCurrency).Replace("{Date}", fxTradeNotification.FxRateLog.CreatedAt.ToString("MMM dd, yyyy")).Replace("{ReferenceNumber}", fxTradeNotification.Reference).Replace("{ToAmount}", fxTradeNotification.FxRateLog.ToAmount.ToString("N2")).Replace("{Rate}", fxTradeNotification.FxRateLog.Rate.ToString()).Replace("{Status}", fxTradeNotification.Status);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = fxTradeNotification.EmailAddress, Subject = EmailSubjects.Transfer, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "TradingAsync", ex);
            }
            return false;
        }

        public async Task<bool> ApprovedTransferAsync(string emailAddress, string firstName)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.TransferApproved, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", firstName).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Transfer, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "ApprovedPayoutAsync", ex);
            }
            return false;
        }

        public async Task<bool> RejectedTransferAsync(string emailAddress, string firstName)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.TransferRejected, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{FirstName}", firstName).Replace("{URL}", StaticData.AppUrl);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.Transfer, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "RejectedPayoutAsync", ex);
            }
            return false;
        }

        public async Task<bool> SendVerificationLinkAsync(string firstName, string emailAddress, string link, string templateId)
        {
            try
            {
                string template = _fileService.GetTemplate(templateId, Templates.Email);

                string emailContent = template.Replace("{VERIFICATIONLINK}", link).Replace("{CUSTOMERNAME}", firstName);
                return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = templateId, Message = emailContent });
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "SendVerificationLink", ex);
            }

            return false;
        }

        public async Task<bool> OnboardingAsync(string customerName, string emailAddress, string templateId)
        {
            try
            {
                string template = _fileService.GetTemplate(templateId, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{CUSTOMERNAME}", customerName);

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = templateId, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Onboarding", ex);
            }
            return false;
        }


        public async Task<bool> FailedVerificationAsync(string firstName, string lastName, string dojahReference, string templateId)
        {
            try
            {
                string customerName = string.Concat(firstName, " ", lastName);

                string template = _fileService.GetTemplate(templateId, Templates.Email);

                //var dojahLink = _apiSettings.Where(x => x.Id == ApplicationSettings.Dojah).FirstOrDefault().Value;

                //var escalations = _apiSettings.Where(x => x.Id == ApplicationSettings.Escalation).FirstOrDefault().Value;

                //if (escalations is null) return false;

                //string subject = templateId.Replace("_", " ");

                //string content = template.Replace("{CUSTOMERNAME}", customerName).Replace("{REFERENCENUMBER}", dojahReference ?? "N/A").Replace("{URL}", dojahLink);

                //  return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver= escalations, Subject= subject, Message= content });
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "FailedVerification", ex);
            }
            return false;
        }

        public async Task<bool> InvitationAsync(string emailAddress, string token, string templateId, string inviterFullName, string inviteeDepartment, string emailSubject)
        {
            try
            {
                string invitationLink = string.Concat(StaticData.InvitationLink, token);
                string invitee = emailAddress[..emailAddress.IndexOf('@')];
                invitee = string.Concat(char.ToUpper(invitee[0]), invitee[1..]);
                string template = _fileService.GetTemplate(templateId, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{URL}", invitationLink);
                    emailContent = emailContent.Replace("{invitee}", invitee);
                    emailContent = emailContent.Replace("{inviter}", inviterFullName);
                    emailContent = emailContent.Replace("{invitee@gmail.com}", emailAddress);
                    emailContent = emailContent.Replace("{dept}", inviteeDepartment);

                    bool send = await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Message = emailContent, Subject = emailSubject });
                    if (send) return true;
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "Invitation", ex);
            }
            return false;
        }

        public async Task<bool> NewDisputeAsync(string accountName, string reference, string type, string currency, decimal amount, string comment, DateTime dateCreated)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.NewDispute, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{AccountName}", accountName).Replace("{Currency}", currency).Replace("{Amount}", amount.ToString("N2")).Replace("{Reference}", reference).Replace("{TransactionType}", type).Replace("{Description}", comment).Replace("{Date}", dateCreated.ToString("MMM dd, yyyy"));

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = StaticData.BackendEmailAddress, Subject = EmailSubjects.Dispute, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "NewDisputeAsync", ex);
            }
            return false;
        }

        public async Task<bool> UpdateDisputeAsync(string email, string reference, string type, string currency, decimal amount, string comment, DateTime dateCreated)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.NewDispute, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{Currency}", currency).Replace("{Amount}", amount.ToString("N2")).Replace("{Reference}", reference).Replace("{TransactionType}", type).Replace("{Description}", comment).Replace("{Date}", dateCreated.ToString("MMM dd, yyyy"));

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = StaticData.BackendEmailAddress, Subject = EmailSubjects.Dispute, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "UpdateDisputeAsync", ex);
            }
            return false;
        }

        public async Task<bool> LowBalanceAsync(string emailAddress, string currency, decimal balance)
        {
            try
            {
                string template = _fileService.GetTemplate(EmailTemplates.LowBalance, Templates.Email);

                if (!string.IsNullOrEmpty(template))
                {
                    string emailContent = template.Replace("{Currency}", currency).Replace("{Balance}", balance.ToString("N2"));

                    return await _mailClientIntegration.SendAsync(new SendMailRequest { Receiver = emailAddress, Subject = EmailSubjects.LowBalance, Message = emailContent });
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("EmailService", "LowBalanceAsync", ex);
            }
            return false;
        }
    }
}
