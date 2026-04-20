using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IReportRepository
    {
        Task<List<BillPaymentDetails>> BillPaymentAsync(GetBillPaymentRequest request);
        Task<List<CardObject>> CardsAsync(GetCardRequest request);
        Task<List<Customer>> CustomersAsync(GetCustomerRequest request);
        Task<List<TransactionDetails>> RevenueAsync(GetRevenueRequest request);
        Task<List<TransactionDetails>> TransactionsAsync(GetTransactionRequest request);
        Task<List<WalletDetails>> WalletsAsync(GetWalletRequest request);
    }
}
