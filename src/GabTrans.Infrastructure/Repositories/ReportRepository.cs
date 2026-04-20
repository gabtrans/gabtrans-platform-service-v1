using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;

namespace GabTrans.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly GabTransContext _context;

        public ReportRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<List<BillPaymentDetails>> BillPaymentAsync(GetBillPaymentRequest request)
        {
            var actorId = 0;
            var statusId = request.TransactionStatusId ?? 0;
            var billCategoryId = request.BillCategoryId ?? 0;
            var billId = request.BillId ?? 0;
            string recipient = request.Recipient == null ? "" : request.Recipient;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            var kyc = await _context.Kycs.Where(x => x.UserId == 1).FirstOrDefaultAsync();

            return await _context.Set<BillPaymentDetails>().FromSqlRaw("CALL sp_GetBillPayments ({0},{1},{2},{3},{4},{5},{6},{7},{8});", actorId, statusId, billCategoryId, billId, kyc.Country, recipient, startDate, endDate, request.Complete).ToListAsync();
        }

        public async Task<List<CardObject>> CardsAsync(GetCardRequest request)
        {
            string currency = request.CurrencyCode == null ? "" : request.CurrencyCode;
            var statusId = request.StatusId ?? 0;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            var kyc = await _context.Kycs.Where(x => x.UserId == 1).FirstOrDefaultAsync();

            return await _context.Set<CardObject>().FromSqlRaw("CALL sp_GetCards ({0},{1},{2},{3},{4},{5});", statusId, currency, kyc.Country, startDate, endDate, request.Complete).ToListAsync();
        }

        public async Task<List<Customer>> CustomersAsync(GetCustomerRequest request)
        {
            var statusId = request.StatusId ?? 0;
            string recipient = request.EmailOrPhone == null ? "" : request.EmailOrPhone;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            var kyc = await _context.Kycs.Where(x => x.UserId == 1).FirstOrDefaultAsync();

            return await _context.Set<Customer>().FromSqlRaw("CALL sp_GetCustomers ({0},{1},{2},{3},{4},{5});", statusId, recipient, kyc.Country, startDate, endDate, request.Complete).ToListAsync();
        }

        public async Task<List<TransactionDetails>> RevenueAsync(GetRevenueRequest request)
        {
            var actorId = 0;
            var statusId = request.TransactionStatusId ?? 0;
            var transferTypeId = request.TransferTypeId ?? 0;
            var paymentTypeId = request.TransferTypeId ?? 0;
            var billCategoryId = request.BillCategoryId ?? 0;
            var billId = request.BillId ?? 0;
            string countryCode = request.CountryCode == null ? "" : request.CountryCode;
            string currencyCode = request.CurrencyCode == null ? "" : request.CurrencyCode;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            return await _context.Set<TransactionDetails>().FromSqlRaw("CALL sp_GetTransactions ({0},{1},{2},{3},{4},{5},{6},{7},{8});", actorId, paymentTypeId, statusId, transferTypeId, countryCode, currencyCode, startDate, endDate, request.Complete).ToListAsync();
        }

        public async Task<List<TransactionDetails>> TransactionsAsync(GetTransactionRequest request)
        {
            var actorId = 0;
            var statusId = request.TransactionStatusId ?? 0;
            var transferTypeId = request.TransferTypeId ?? 0;
            var paymentTypeId = request.TransferTypeId ?? 0;
            var billCategoryId = request.BillCategoryId ?? 0;
            var billId = request.BillId ?? 0;
            string countryCode = request.CountryCode == null ? "" : request.CountryCode;
            string currencyCode = request.CurrencyCode == null ? "" : request.CurrencyCode;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            return await _context.Set<TransactionDetails>().FromSqlRaw("CALL sp_GetTransactions ({0},{1},{2},{3},{4},{5},{6},{7},{8});", actorId, paymentTypeId, statusId, transferTypeId, countryCode, currencyCode, startDate, endDate, request.Complete).ToListAsync();
        }

        public async Task<List<WalletDetails>> WalletsAsync(GetWalletRequest request)
        {
            string currency = request.CurrencyCode == null ? "" : request.CurrencyCode;
            string recipient = request.EmailOrPhone == null ? "" : request.EmailOrPhone;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            var kyc = await _context.Kycs.Where(x => x.UserId == 1).FirstOrDefaultAsync();

            return await _context.Set<WalletDetails>().FromSqlRaw("CALL sp_GetWallets ({0},{1},{2},{3},{4},{5});", currency, kyc.Country, recipient, startDate, endDate, request.Complete).ToListAsync();
        }
    }
}
