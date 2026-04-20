using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Notification
{
    public interface ISmsNotificationService
    {
        Task<ApiResponse> SendAsync(SendSmsRequest smsRequest);
        Task SendOtpAsync(string phoneNumber, string otp, string smsTemplateId, string languageCode, string countryCode);
        Task SendOtpAsync(string phoneNumber, string otp, decimal amount, string product, string recipient, string smsTemplateId, string languageCode, string countryCode);
        Task SendOtpAsync(string phoneNumber, string otp, decimal amount, string beneficiaryName, string bank, string accountNo, string smsTemplateId, string languageCode, string countryCode);
        Task ReceiveMoneyAsync(string firstName, string phoneNumber, string accountNumber, string accountName, string routingCode, string bank, string bankCountry, string currency, decimal amount, string referenceNumber, long paymentTypeId, string smsTemplateId, string languageCode, string countryCode);
    }
}
