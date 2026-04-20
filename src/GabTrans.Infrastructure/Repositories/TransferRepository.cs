using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class TransferRepository(GabTransContext context, ILogService logService) : ITransferRepository
    {
        private readonly ILogService _logService = logService;
        private readonly GabTransContext _context = context;

        public async Task<Transfer> DetailsAsync(long id)
        {
            return await _context.Transfers.FindAsync(id);
        }

        public async Task<Transfer> DetailsAsync(string reference)
        {
            return await _context.Transfers.Where(x => x.Reference == reference).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(Transfer transfer)
        {
            //  using var tranContext = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Transfers.Add(transfer);
                await _context.SaveChangesAsync();
                //  await tranContext.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransferRepository", "InsertAsync", ex);
                // await tranContext.RollbackAsync();
            }
            return false;
        }

        public async Task<IEnumerable<Transfer>> GetByStatusAsync(string status)
        {
            return await _context.Transfers.Where(x => x.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> GetAsync(string status, string gateway)
        {
            return await _context.Transfers.Where(x => x.Status == status && x.Gateway == gateway).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Transfer transfer)
        {
            using var tranContext = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Transfers.Update(transfer);
                await _context.SaveChangesAsync();
                await tranContext.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("TransferRepository", "UpdateAsync", ex);
                await tranContext.RollbackAsync();
            }
            return false;
        }

        public async Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryTransaction.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(queryTransaction.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryTransaction.EndDate) ? DateTime.Now : Convert.ToDateTime(queryTransaction.EndDate);

            return await (from t in _context.Transfers.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == t.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                         (queryTransaction.AccountId == 0 || queryTransaction.AccountId == null || a.Id == queryTransaction.AccountId) &&
                          (string.IsNullOrEmpty(queryTransaction.AccountName) || a.Name.Contains(queryTransaction.AccountName)) &&
                          (string.IsNullOrEmpty(queryTransaction.Email) || u.EmailAddress == queryTransaction.Email) &&
                          (string.IsNullOrEmpty(queryTransaction.Status) || t.Status == queryTransaction.Status)
                          && (string.IsNullOrEmpty(queryTransaction.Reference) || t.Reference == queryTransaction.Reference)
                            && t.CreatedAt >= fromDate && t.CreatedAt <= toDate
                          select new TransactionModel
                          {
                              Id = t.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                              Reference = t.Reference,
                              TransactionType = TransactionTypes.Transfer,
                              Amount = $"{t.Currency}{t.Amount.ToString()}",
                              Status = t.Status
                          }).ToListAsync();
        }

        public async Task<List<TransactionModel>> GetAsync(QueryTransfer queryTransfer)
        {
            DateTime fromDate = string.IsNullOrEmpty(queryTransfer.StartDate) ? DateTime.Now.AddDays(-7).Date : Convert.ToDateTime(queryTransfer.StartDate);
            DateTime toDate = string.IsNullOrEmpty(queryTransfer.EndDate) ? DateTime.Now : Convert.ToDateTime(queryTransfer.EndDate);

            return await (from t in _context.Transfers.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == t.AccountId).DefaultIfEmpty()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          where
                          (string.IsNullOrEmpty(queryTransfer.AccountName) || a.Name.Contains(queryTransfer.AccountName)) &&
                          (string.IsNullOrEmpty(queryTransfer.Email) || u.EmailAddress == queryTransfer.Email) &&
                          (string.IsNullOrEmpty(queryTransfer.Status) || t.Status == queryTransfer.Status)
                          && (string.IsNullOrEmpty(queryTransfer.Reference) || t.Reference == queryTransfer.Reference)
                            && t.CreatedAt >= fromDate && t.CreatedAt <= toDate
                          select new TransactionModel
                          {
                              Id = t.Id,
                              AccountId = a.Id,
                              AccountName = a.Name,
                              Email = u.EmailAddress,
                              TransactionDate = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                              Reference = t.Reference,
                              TransactionType = TransactionTypes.Transfer,
                              Amount = $"{t.Currency}{t.Amount.ToString()}",
                              Status = t.Status
                          }).OrderByDescending(t => t.Id).ToListAsync();
        }

        public async Task<long> GetFundTransferAsync()
        {
            return await _context.Transfers.Where(p => p.Status == TransactionStatuses.Successful).CountAsync();
        }

        public async Task<List<IdNameObject>> GetBankAccountTypesAsync()
        {
            return await _context.BankAccountTypes.Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<AccountType> GetEntityAsync(string name)
        {
            return await _context.AccountTypes.AsNoTracking().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<IdNameObject>> GetReasonsAsync()
        {
            return await _context.PaymentReasons.Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<PaymentReason> GetReasonAsync(string name)
        {
            return await _context.PaymentReasons.AsNoTracking().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<TransferModel> DetailsByIdAsync(long id)
        {
            return await (from p in _context.Transfers.AsNoTracking()
                          from r in _context.TransferRecipients.AsNoTracking().Where(r => r.Id == p.TransferRecipientId).DefaultIfEmpty()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.Id == p.AccountId).DefaultIfEmpty()
                          from itc in _context.Countries.AsNoTracking().Where(itc => itc.Code == r.IntermediaryBankCountry).DefaultIfEmpty()
                          from its in _context.States.AsNoTracking().Where(its => its.Code == r.IntermediaryState).DefaultIfEmpty()
                          from sc in _context.Countries.AsNoTracking().Where(sc => sc.Code == r.Country).DefaultIfEmpty()
                          from bc in _context.Countries.AsNoTracking().Where(bc => bc.Code == r.BankCountry).DefaultIfEmpty()
                          from ss in _context.States.AsNoTracking().Where(ss => ss.Code == r.State).DefaultIfEmpty()
                          from bs in _context.States.AsNoTracking().Where(bs => bs.Code == r.BankState).DefaultIfEmpty()
                          where p.Id == id
                          select new TransferModel
                          {
                              Sender = a.Name,
                              AccountName = r.AccountName,
                              AccountNumber = r.AccountNumber,
                              AccountRoutingType = r.AccountRoutingType,
                              Amount = p.Amount,
                              BankAccountType = r.BankAccountType,
                              BankBranch = r.BankBranch,
                              BankCity = r.BankCity,
                              BankCountry = bc == null ? "" : bc.Name,
                              BankName = r.BankName,
                              BankPostalCode = r.BankPostalCode,
                              BankState = bs == null ? "" : bs.Name,
                              BankStreetAddress = r.BankStreetAddress,
                              City = r.City,
                              Country = sc == null ? "" : sc.Name,
                              Currency = p.Currency,
                              DateOfBirth = r.DateOfBirth,
                              Email = r.Email,
                              Iban = r.AccountNumber,
                              IntermediaryBankCountry = itc == null ? "" : itc.Name,
                              IntermediaryBankName = r.IntermediaryBankName,
                              IntermediaryCity = r.IntermediaryCity,
                              IntermediaryPostalCode = r.IntermediaryPostalCode,
                              IntermediaryRoutingCode = r.IntermediaryRoutingCode,
                              IntermediaryState = its == null ? "" : its.Name,
                              IntermediaryStreet1 = r.IntermediaryStreet1,
                              IntermediaryStreet2 = r.IntermediaryStreet2,
                              InternationalBankName = r.InternationalBankName,
                              Name = r.Name,
                              PaymentMethod = r.PaymentMethod,
                              PhoneNumber = r.PhoneNumber,
                              PostCode = r.PostCode,
                              Reason = p.Reason,
                              RecipientAccountType = r.Type,
                              RoutingNumber = r.RoutingNumber,
                              State = ss == null ? "" : ss.Name,
                              TransactionStatus = r.Status,
                              StreetAddress = r.StreetAddress,
                              SwiftCode = r.SwiftCode,
                              TransactionType = TransactionTypes.Transfer,
                              SenderAccountType = r.BankAccountType,
                              TransactionData = r.CreatedAt,
                              TransactionReceiptName = r.AccountName,
                              TransactionReference = p.Reference
                          }).FirstOrDefaultAsync();
        }


        public async Task<List<SummaryValue>> RevenuesAsync()
        {
            var transfers = await _context.Transfers
            .Where(s => s.CreatedAt.Year == DateTime.Now.Year)
            .GroupBy(s => new
            {
                s.CreatedAt.Month
            }).Select(g => new
            {
                Month = g.Key.Month,
                TotalFee = g.Sum(s => s.Fee)
            }).ToListAsync(); // Materialize the query

            return transfers
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
