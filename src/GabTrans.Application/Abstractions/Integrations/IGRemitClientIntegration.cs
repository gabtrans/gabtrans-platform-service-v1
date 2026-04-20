using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GabTrans.Application.DataTransfer.GRemit;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IGRemitClientIntegration
    {
        Task<GRemitUpdateTransactionResponse> ApproveAsync(GremitAccount gremitApplication, string referenceNumber, string paidDate);
        Task<GRemitUpdateTransactionResponse> RejectAsync(GremitAccount gremitApplication, string referenceNumber, string reason);
        Task<GRemitTransactionsResponse> GetTransactionsAsync(GremitAccount gremitApplication);
    }
}
