using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Notification;

namespace GabTrans.Infrastructure.Notification
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly ICountryRepository _countryRepository;
        private readonly IValidationService _validationService;
       // private readonly IHiBudPayClientIntegration _hiBudPayIntegration;


        public SmsNotificationService(ILogService logService, IFileService fileService, IValidationService validationService, ICountryRepository countryRepository)
        {
            _logService = logService;
            _fileService = fileService;
            _validationService = validationService;
           // _hiBudPayIntegration = hiBudPayIntegration;
            _countryRepository = countryRepository;
        }


        public async Task SendOtpAsync(string phoneNumber, string otp, string smsTemplateId, string languageCode, string countryCode)
        {
            try
            {
                string template = _fileService.GetTemplate(smsTemplateId, Templates.Sms);

                string content = template.Replace("{OTP}", otp).Replace("{HELPLINE}", StaticData.HotLine);

               // await _hiBudPayIntegration.SendSmsAsync(new SendSmsRequest { PhoneNumber= phoneNumber, Message= content, CountryCode=countryCode, Subject=smsTemplateId });
            }
            catch (Exception ex)
            {
                _logService.LogError("SmsService", "SendOtp", ex);
            }
        }


        public async Task SendOtpAsync(string phoneNumber, string otp, decimal amount, string product, string recipient, string smsTemplateId, string languageCode, string countryCode)
        {
            try
            {
                string template = _fileService.GetTemplate(smsTemplateId, Templates.Sms);

                string content = template.Replace("{OTP}", otp).Replace("{AMOUNT}", amount.ToString("N2")).Replace("{PRODUCTNAME}", product).Replace("{RECIPIENT}", recipient).Replace("{HELPLINE}", StaticData.HotLine);

                phoneNumber = phoneNumber.Substring(2, 10);
                if (countryCode.ToUpper().Equals(Countries.United_Kingdom)) phoneNumber = string.Concat("234", phoneNumber);
              //  await _hiBudPayIntegration.SendSmsAsync(new SendSmsRequest { PhoneNumber= phoneNumber, Message= content, CountryCode=countryCode });
            }
            catch (Exception ex)
            {
                _logService.LogError("SmsService", "SendOtp", ex);
            }
        }


        public async Task SendOtpAsync(string phoneNumber, string otp, decimal amount, string beneficiaryName, string bank, string accountNo, string smsTemplateId, string languageCode, string countryCode)
        {
            try
            {
                string template = _fileService.GetTemplate(smsTemplateId, Templates.Sms);

                string content = template.Replace("{OTP}", otp).Replace("{BENEFICIARYNAME}", beneficiaryName).Replace("{BANK}", bank).Replace("{AMOUNT}", amount.ToString("N2")).Replace("{HELPLINE}", StaticData.HotLine);

                phoneNumber = phoneNumber.Substring(2, 10);
                if (countryCode.ToUpper().Equals(Countries.United_Kingdom)) phoneNumber = string.Concat("234", phoneNumber);

               // await _hiBudPayIntegration.SendSmsAsync(new SendSmsRequest { PhoneNumber= phoneNumber, Message= content, CountryCode=countryCode });
            }
            catch (Exception ex)
            {
                _logService.LogError("SmsService", "SendOtp", ex);
            }
        }


        public async Task ReceiveMoneyAsync(string firstName, string phoneNumber, string accountNumber, string accountName, string routingCode, string bank, string bankCountry, string currency, decimal amount, string referenceNumber, long paymentTypeId, string smsTemplateId, string languageCode, string countryCode)
        {
            try
            {
                string template = _fileService.GetTemplate(smsTemplateId, Templates.Sms);

                if (string.IsNullOrEmpty(firstName)) firstName = "Sir/Ma";

                if (string.IsNullOrEmpty(routingCode)) routingCode = "N/A";

                string content = template.Replace("{FirstName}", firstName).Replace("{Support}", StaticData.HotLine).Replace("{AccountNumber}", accountNumber).Replace("{Amount}", string.Concat(currency, amount.ToString())).Replace("{AccountName}", accountName).Replace("{Bank}", bank).Replace("{RoutingCode}", routingCode).Replace("{ReferenceNumber}", referenceNumber).Replace("{Bank Country}", bankCountry).Replace("{{Routing Code}}", paymentTypeId == PaymentTypes.International ? "Swift Code" : "Routing Code").Replace("{{Account Number}}", paymentTypeId == PaymentTypes.International ? "IBAN Number" : "Account Number");

                if (paymentTypeId == PaymentTypes.International) content.Replace("Routing Code", "SwiftCode").Replace("Account Number", "IBAN Number");

                if (countryCode.ToUpper().Equals(Countries.United_Kingdom)) phoneNumber = string.Concat("234", phoneNumber);

              //  await _hiBudPayIntegration.SendSmsAsync(new SendSmsRequest { PhoneNumber= phoneNumber, Message= content, CountryCode=countryCode });
            }
            catch (Exception ex)
            {
                _logService.LogError("SmsService", "ReceiveMoney", ex);
            }
        }

        public async Task<ApiResponse> SendAsync(SendSmsRequest smsRequest)
        {
            var result = new ApiResponse();
            try
            {
                result = _validationService.SendSms(smsRequest);
                if (!result.Success) return result;
                var country = await _countryRepository.DetailsAsync(smsRequest.CountryCode);
                if (country is null)
                {
                    return new ApiResponse
                    {
                        
                        Message = "Invalid countryCode"
                    };
                }

                smsRequest.PhoneNumber = string.Concat(country.Dial, smsRequest.PhoneNumber.Substring(1));

                //bool send = await _hiBudPayIntegration.SendSmsAsync(smsRequest);
                //if (!send)
                //{
                //    return new ApiResponse
                //    {
                        
                //        Message = "Unable to process request"
                //    };
                //}

                result.Success = true;
                
                result.Message = "Message successfully sent";
            }
            catch (Exception ex)
            {
                _logService.LogError("SmsService", "Send", ex);
                
                result.Message = "Kindly try again later";
            }
            return result;
        }
    }
}
