using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;


namespace GabTrans.Infrastructure.Repositories
{
    public class DepositRepository(GabTransContext context, ILogService logService) : IDepositRepository
    {
        private readonly ILogService _logService = logService;
        private readonly GabTransContext _context = context;

        public async Task<bool> UpdateStatusAsync(long id, string status, bool refunded, string? gatewayReference = null)
        {
            using var tranContext = await _context.Database.BeginTransactionAsync();

            try
            {
                var transaction = await _context.Deposits.FindAsync(id);
                if (transaction is null) return false;
                transaction.Status = status;
                //transaction.Refunded = refunded;
                if (gatewayReference != null) transaction.GatewayReference = gatewayReference;
                if (status == TransactionStatuses.Successful) transaction.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                await tranContext.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransactionRepository", "UpdateStatusAsync", ex);
                await tranContext.RollbackAsync();
            }
            return false;
        }

        public async Task<bool> UpdateAsync(long id, string status, string responseMessage, string? gatewayResponse, bool refunded, string? tranRef = null)
        {
            using var tranContext = await _context.Database.BeginTransactionAsync();

            try
            {
                var transaction = await _context.Deposits.FindAsync(id);
                if (transaction is null) return false;
                transaction.Status = status;
                transaction.ResponseMessage = responseMessage;
                transaction.GatewayResponse = gatewayResponse;
                /// transaction.Refunded = refunded;
                if (tranRef != null) transaction.GatewayReference = tranRef;
                if (status == TransactionStatuses.Successful) transaction.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                await tranContext.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransactionRepository", "UpdateAsync", ex);
                await tranContext.RollbackAsync();
            }
            return false;
        }

        public async Task<List<IdNameObject>> GetPaymentsAsync()
        {
            return await _context.PaymentMethods.Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        //public async Task<List<PaymentType>> GetPaymentTypesAsync()
        //{
        //    return await _context.PaymentTypes.ToListAsync();
        //}

        public async Task<List<IdNameObject>> GetAccountTypesAsync()
        {
            return await _context.AccountTypes.Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<Deposit> DetailsByTranRefAsync(string tranReference)
        {
            return await _context.Deposits.FirstOrDefaultAsync(x => x.GatewayReference == tranReference);
        }

        //public async Task<List<BankModel>> GetBanksAsync(string countryCode)
        //{
        //    return await _context.Banks.Where(x => x.CountryCode == countryCode && x.Approved).Select(x => new BankModel { Code = x.Code, Name = x.Name, Logo = x.Logo }).OrderBy(x => x.Name).ToListAsync();
        //}

        //public async Task<List<IdNameCode>> GetBankListAsync(string countryCode)
        //{
        //    return await _context.Banks.Where(x => x.CountryCode == countryCode && x.Approved).OrderBy(x => x.Name).Select(a => new IdNameCode { Id = a.Id, Code = a.Code, Name = a.Name }).ToListAsync();
        //}

        //public async Task<bool> IsBankValidAsync(string bankCode)
        //{
        //    return await _context.Banks.AnyAsync(x => x.Code == bankCode);
        //}

        //public async Task<Bank> BankDetailsByCodeAsync(string bankCode)
        //{
        //    return await _context.Banks.Where(x => x.Code == bankCode).FirstOrDefaultAsync();
        //}


        //public async Task<Bank> BankDetailsByNameAsync(string name)
        //{
        //    return await _context.Banks.Where(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public async Task<List<TransactionHistory>> GetTransactionsAsync(GetTransactionRequest request)
        {
            var actorId = 0;
            var accountId = 0;
            var statusId = request.TransactionStatusId ?? 0;
            var transferTypeId = request.TransferTypeId ?? 0;
            var paymentTypeId = request.TransferTypeId ?? 0;
            var billCategoryId = request.BillCategoryId ?? 0;
            var billId = request.BillId ?? 0;
            string countryCode = request.CountryCode ?? "";
            string currencyCode = request.CurrencyCode == null ? "" : request.CurrencyCode;
            string startDate = request.StartDate == null ? "" : request.StartDate;
            string endDate = request.EndDate == null ? "" : request.EndDate;

            var kyc = await _context.Kycs.Where(x => x.UserId == 1).FirstOrDefaultAsync();

            //if (kyc is not null) countryCode = kyc.CountryCode;

            string query = "CALL sp_GetTransactionHistories ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10});";

            return await _context.Set<TransactionHistory>().FromSqlRaw(query, accountId, paymentTypeId, statusId, transferTypeId, billCategoryId, billId, countryCode, currencyCode, startDate, endDate, request.Complete).ToListAsync();
        }

        //public async Task<TransactionDetails> DetailsAsync(long id)
        //{
        //    return new TransactionDetails(); // await _context.TransactionDetails.FromSqlRaw("CALL sp_GetTransactionDetails {0}", id).FirstOrDefaultAsync();
        //}

        public async Task<List<IdNameObject>> GetTransactionTypesAsync()
        {
            return await _context.TransactionTypes.Select(x => new IdNameObject { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        //public async Task<List<TransactionStatus>> GetTransactionStatusAsync()
        //{
        //    return await _context.TransactionStatuses.ToListAsync();
        //}

        public async Task<VirtualAccountObject> GetCustomerAccountAsync(long accountId, string bankName)
        {
            return await _context.VirtualAccounts.Where(x => x.BankName == bankName && x.AccountId == accountId).Select(a => new VirtualAccountObject { AccountNumber = a.AccountNumber, AccountName = a.Account.Name }).FirstOrDefaultAsync();
        }

        //public async Task<PaymentType> GetPaymentTypeAsync(long id)
        //{
        //    return await _context.PaymentTypes.FirstOrDefaultAsync(x => x.Id == id);
        //}

        //public async Task<Bank> BankDetailsByIdAsync(long bankId)
        //{
        //    return await _context.Banks.FirstOrDefaultAsync(x => x.Id == bankId);
        //}

        public async Task<Deposit> DetailsByReferenceAsync(string referenceNumber)
        {
            return await _context.Deposits.FirstOrDefaultAsync(x => x.Reference == referenceNumber);
        }

        public async Task<long> InsertAsync(Deposit transaction)
        {
            using var tranContext = await _context.Database.BeginTransactionAsync();
            long insertResult = 0;
            try
            {
                _context.Deposits.Add(transaction);
                await _context.SaveChangesAsync();
                await tranContext.CommitAsync();
                insertResult = transaction.Id;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransactionRepository", "InsertAsync", ex);
                await tranContext.RollbackAsync();
            }
            return insertResult;
        }

        public async Task<List<Deposit>> GetAsync(GetTransactionHistoryRequest request)
        {
            DateTime startDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(request.StartDate);
            DateTime endDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now.AddDays(1) : Convert.ToDateTime(request.EndDate);

            return await _context.Deposits.AsNoTracking().Where(w => w.AccountId == request.AccountId && w.CreatedAt >= startDate && w.CreatedAt < endDate).ToListAsync();
        }

        public async Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryTransaction.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(queryTransaction.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryTransaction.EndDate) ? DateTime.Now : Convert.ToDateTime(queryTransaction.EndDate);

            return await (from d in _context.Deposits.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == d.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                      (queryTransaction.AccountId == 0 || queryTransaction.AccountId == null || a.Id == queryTransaction.AccountId) &&
                          (string.IsNullOrEmpty(queryTransaction.AccountName) || a.Name.Contains(queryTransaction.AccountName)) &&
                            (string.IsNullOrEmpty(queryTransaction.Email) || u.EmailAddress == queryTransaction.Email) &&
                            (string.IsNullOrEmpty(queryTransaction.Status) || d.Status == queryTransaction.Status)
                            && (string.IsNullOrEmpty(queryTransaction.Reference) || d.Reference == queryTransaction.Reference)
                              && d.CreatedAt >= fromDate && d.CreatedAt <= toDate
                          select new TransactionModel
                          {
                              Id = d.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                              Reference = d.Reference,
                              TransactionType = TransactionTypes.Deposit,
                              Amount = $"{d.Currency}{d.Amount.ToString()}",
                              Status = d.Status
                          }).ToListAsync();
        }

        public async Task<DepositModel> DetailsAsync(long id)
        {
            return await (from d in _context.Deposits.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == d.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where d.Id == id

                          select new DepositModel
                          {
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                              Reference = d.Reference,
                              TransactionType = TransactionTypes.Deposit,
                              Amount = $"{d.Currency}{d.Amount.ToString()}",
                              Status = d.Status,
                              Fee = d.Fee,
                              Id = id,
                              Currency = d.Currency,
                              PayerAccountName = d.PayerAccountName,
                              PayerAccountNumber = d.PayerAccountNumber,
                              PayerBank = d.PayerBank,
                              PayerCountry = d.PayerCountry,
                              SettledAmount = $"{d.Currency}{d.SettledAmount.ToString()}",
                              Narration = d.Narration,
                              Type = d.Type,

                          }).FirstOrDefaultAsync();
        }

        public async Task<List<SummaryValue>> RevenuesAsync()
        {
            var deposits = await _context.Deposits
            .Where(s => s.CreatedAt.Year == DateTime.Now.Year)
            .GroupBy(s => new
            {
                s.CreatedAt.Month
            }).Select(g => new
            {
                Month = g.Key.Month,
                TotalFee = g.Sum(s => s.Fee)
            }).ToListAsync(); // Materialize the query

            return deposits
                .Select(g => new SummaryValue
                {
                    Name = new DateTime(DateTime.Now.Year, g.Month, 1).ToString("MMM"),
                    Value = g.TotalFee.ToString("N2")
                })
                .OrderBy(r => r.Name)
                .ToList();
        }
    }
}

