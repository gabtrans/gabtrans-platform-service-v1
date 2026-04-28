using GabTrans.Application.Abstractions.Integrations;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer.GRemit;
using GabTrans.Domain.Entities;
using Gremit;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GabTrans.Infrastructure.Integrations
{
    public class GRemitClientIntegration(ILogService logService) : IGRemitClientIntegration
    {
        private readonly ILogService _logService = logService;

        public async Task<GRemitTransactionsResponse> GetTransactionsAsync(GremitAccount gremitApplication)
        {
            var response = new GRemitTransactionsResponse();

            try
            {
                DateOnly startDateFormatted = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
                DateOnly endDateFormatted = DateOnly.FromDateTime(DateTime.Now);

                string startDate = startDateFormatted.ToString("yyyy-MM-dd");
                string endDate = endDateFormatted.ToString("yyyy-MM-dd");

                string body = $@"
                            <Data>
                                <Head>
                                    <GUID>{gremitApplication.Guid}</GUID>
                                    <UserID>{gremitApplication.Username}</UserID>
                                    <Password>{gremitApplication.Password}</Password>
                                    <LocationCode>{gremitApplication.Location}</LocationCode>
                                </Head>
                                <Body>
                                    <Transaction>
                                        <StartDate>{startDate}</StartDate>
                                        <EndDate>{endDate}</EndDate>
                                        <DeliveryMethodCode>{gremitApplication.DeliveryMethod.ToUpper()}</DeliveryMethodCode>
                                    </Transaction>
                                </Body>
                            </Data>
                        ";

               // _logService.LogInfo("GRemitClientIntegration", $"GetPendingTransactionsAsync Request to GRemit for {gremitApplication.AccountId}:: ", body);

                var soapServiceChannel = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                var gremitResponse = await soapServiceChannel.Transaction_Select_Unpaid_By_DateRangeAsync(body);
                if (gremitResponse is null) return new GRemitTransactionsResponse();

                _logService.LogInfo("GRemitClientIntegration", "GetPendingTransactionsAsync Response from GRemit:: ", JsonConvert.SerializeObject(gremitResponse));

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(gremitResponse.Body.Transaction_Select_Unpaid_By_DateRangeResult);
                if (xmlDocument.InnerText.Contains("No Transactions found for the provided parameters")) return new GRemitTransactionsResponse();

                //var resp = File.ReadAllText(@"C:\Users\FALADE\Desktop\BudPay\GRemit\\single.xml");
                //var xmlDocument = new XmlDocument();
                //xmlDocument.LoadXml(resp);
                //if (xmlDocument.InnerText.Contains("No Transactions found for the provided parameters")) return new GRemitTransactionsResponse();
               // _logService.LogInfo("GRemitClientIntegration", "GetPendingTransactionsAsync Raw Response from GRemit:: ", xmlDocument.InnerXml);

                string responseDetails = JsonConvert.SerializeObject(xmlDocument);

                try
                {
                    response = JsonConvert.DeserializeObject<GRemitTransactionsResponse>(responseDetails);
                }
                catch (Exception)
                {
                    var singleResponse = JsonConvert.DeserializeObject<GRemitTransactionResponse>(responseDetails);
                    if (singleResponse is null) return new GRemitTransactionsResponse();

                    var result = new GRemitTransactionsResult
                    {
                        ResultCode = singleResponse.Result.ResultCode,
                        RecordCount = singleResponse.Result.RecordCount,
                        Message = singleResponse.Result.Message,
                        Details = new GRemitTransactionsDetails { Transaction = new List<GRemitTransaction> { singleResponse.Result.Details.Transaction } }
                    };

                    return new GRemitTransactionsResponse
                    {
                        Result = result
                    };
                }

                if (response is null || response.Result.RecordCount is null) return new GRemitTransactionsResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitClientIntegration", "GetTransactionsAsync", ex);
            }

            return new GRemitTransactionsResponse();
        }

        public async Task<GRemitUpdateTransactionResponse> ApproveAsync(GremitAccount gremitApplication, string referenceNumber, string paidDate)
        {

            try
            {
                string body = $@"
                            <Data>
                                <Head>
                                     <GUID>{gremitApplication.Guid}</GUID>
                                    <UserID>{gremitApplication.Username}</UserID>
                                    <Password>{gremitApplication.Password}</Password>
                                    <LocationCode>{gremitApplication.Location}</LocationCode>
                                </Head>
                                <Body>
                                  <Transaction>
                                  <ReferenceNo>{referenceNumber}</ReferenceNo>
                                  <PaidDate>{paidDate}</PaidDate>
                                    </Transaction>
                                </Body>
                            </Data>
                        ";

               // _logService.LogInfo("GRemitClientIntegration", "ApproveAsync Request from GRemit:: ", JsonConvert.SerializeObject(body));

                var soapServiceChannel = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                var gremitResponse = await soapServiceChannel.Transaction_Update_MarkAsPaidAsync(body);
                //if (gremitResponse is null) return new GRemitTestResponse();

                _logService.LogInfo("GRemitClientIntegration", "ApproveAsync Response from GRemit:: ", JsonConvert.SerializeObject(gremitResponse));

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(gremitResponse.Body.Transaction_Update_MarkAsPaidResult);
                string responseDetails = JsonConvert.SerializeObject(xmlDocument);
                var response = JsonConvert.DeserializeObject<GRemitUpdateTransactionResponse>(responseDetails);

                if (response is null) return new GRemitUpdateTransactionResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitClientIntegration", "ApproveAsync", ex);
            }

            return new GRemitUpdateTransactionResponse();
        }

        public async Task<GRemitUpdateTransactionResponse> RejectAsync(GremitAccount gremitApplication, string referenceNumber, string reason)
        {
            _logService.LogInfo("GRemitClientIntegration", "About to Reject transaction with reference:: ", referenceNumber);

            try
            {
                string body = $@"
                            <Data>
                                <Head>
                                   <GUID>{gremitApplication.Guid}</GUID>
                                    <UserID>{gremitApplication.Username}</UserID>
                                    <Password>{gremitApplication.Password}</Password>
                                    <LocationCode>{gremitApplication.Location}</LocationCode>
                                </Head>
                                <Body>
                                  <Transaction>
                                  <ReferenceNo>{referenceNumber}</ReferenceNo>
                                  <ReasonForRejection>{reason}</ReasonForRejection>
                                    </Transaction>
                                </Body>
                            </Data>
                        ";

                var soapServiceChannel = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                var gremitResponse = await soapServiceChannel.Transaction_Update_MarkAsRejectedAsync(body);
                //if (gremitResponse is null) return new GRemitTestResponse();

               // _logService.LogInfo("GRemitClientIntegration", "RejectAsync Response from GRemit:: ", JsonConvert.SerializeObject(gremitResponse));

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(gremitResponse.Body.Transaction_Update_MarkAsRejectedResult);
                string responseDetails = JsonConvert.SerializeObject(xmlDocument);
                var response = JsonConvert.DeserializeObject<GRemitUpdateTransactionResponse>(responseDetails);

                if (response is null) return new GRemitUpdateTransactionResponse();

                return response;
            }
            catch (Exception ex)
            {
                _logService.LogError("GRemitClientIntegration", "RejectAsync", ex);
            }

            return new GRemitUpdateTransactionResponse();
        }
    }
}
