using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Models;
using GabTrans.Domain.Constants;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Enums;
using GabTrans.Application.DataTransfer;

namespace GabTrans.Infrastructure.Repositories
{
    public class AccountRepository(GabTransContext context, ILogService logService) : IAccountRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<long> InsertAsync(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return account.Id;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRepository), nameof(InsertAsync), ex);
            }

            return 0;
        }

        public async Task<bool> UpdateAsync(Account account)
        {
            try
            {
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<bool> BulkUpdateAsync(List<Account> accounts)
        {
            try
            {
                _context.Accounts.UpdateRange(accounts);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AccountRepository), nameof(BulkUpdateAsync), ex);
            }

            return false;
        }

        public async Task<Account> DetailsAsync(long id)
        {
            return await _context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> GetExistingAccountsAsync(long userId)
        {
            return await _context.Accounts.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<Account>> GetAsync(string status)
        {
            return await _context.Accounts.AsNoTracking().Where(x => x.Status == status).ToListAsync();
        }

        public async Task<List<Account>> GetExistingAccountsAsync(long userId, string accountType)
        {
            return await _context.Accounts.Where(x => x.UserId == userId && x.Type == accountType).ToListAsync();
        }

        public async Task<bool> DoesNameExistAsync(string name)
        {
            return await _context.Accounts.AnyAsync(x => x.Name == name);
        }

        public async Task<AccountType> AccountTypeAsync(long id)
        {
            return await _context.AccountTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AccountType> AccountTypeAsync(string name)
        {
            return await _context.AccountTypes.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<IdNameObject>> AccountTypesAsync()
        {
            return await _context.AccountTypes.Select(x => new IdNameObject
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
        }

        public async Task<List<Account>> GetAccountsAsync(long userId)
        {
            return await _context.Accounts.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Account> DetailsByUuidAsync(string uuid)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Uuid == uuid);
        }

        public async Task<Account> GetAccountAsync(long userId)
        {
            return await _context.Accounts.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Account>> WithoutVirtualAsync()
        {
            return await (from a in _context.Accounts.AsNoTracking()
                          from v in _context.VirtualAccounts.AsNoTracking().Where(v => v.AccountId == a.Id).DefaultIfEmpty()
                          where a.Status == "" && v.AccountNumber == null
                          select a
                          ).ToListAsync();
        }

        public async Task<List<AccountBalanceModel>> GetAccountBalancesAsync(long userId)
        {
            return await (from a in _context.Accounts.AsNoTracking()
                          from w in _context.Wallets.AsNoTracking().Where(w => w.AccountId == a.Id).DefaultIfEmpty()
                          from v in _context.VirtualAccounts.AsNoTracking().Where(v => v.AccountId == a.Id).DefaultIfEmpty()
                          where a.UserId == userId && a.Status == AccountStatuses.Active
                          select new AccountBalanceModel
                          {
                              Id = w.Id,
                              AccountName = a.Name,
                              Assets = w.Asset,
                              Currency = w.Currency,
                              AvailableBalance = w.Balance
                          }).ToListAsync();
        }

        public async Task<AccountDetailsModel> GetAccountDetailsAsync(long accountId)
        {
            return await (from a in _context.Accounts.AsNoTracking()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          from k in _context.Kycs.AsNoTracking().Where(k => k.UserId == u.Id).DefaultIfEmpty()
                          from c in _context.Countries.AsNoTracking().Where(c => c.Code == k.Country).DefaultIfEmpty()
                          from s in _context.States.AsNoTracking().Where(s => s.Code == k.ResidentialState && s.CountryCode == k.Country).DefaultIfEmpty()
                          where a.Id == accountId && a.Status == AccountStatuses.Active
                          select new AccountDetailsModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                              EmailAddress = u == null ? "" : u.EmailAddress,
                              Type = a == null ? "" : a.Type,
                              CreatedAt = a == null ? "" : a.CreatedAt.ToString("yyyy-MM-dd"),

                              PersonalInformation = new PersonalInformationModel
                              {
                                  PhoneNumber = u == null ? "" : u.PhoneNumber,
                                  BankStatement = k == null ? "" : k.BankStatement,
                                  CitizenShip = k == null ? "" : c.Name,
                                  City = k == null ? "" : k.City,
                                  DateOfBirth = k == null ? "" : k.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  Line1 = k == null ? "" : k.Address1,
                                  Line2 = k == null ? "" : k.Address2,
                                  State = k == null ? "" : s.Name,
                                  Status = k == null ? "" : k.Status,
                                  TaxDocuments = k == null ? "" : k.TaxDocument,
                              },
                              EmploymentInformation = new EmploymentInformationModel
                              {
                                  AnnualIncome = k == null ? "" : k.AnnualIncome,
                                  Employer = k == null ? "" : k.Employer,
                                  EmployerCountry = k == null ? "" : c.Name,
                                  EmployerState = k == null ? "" : s.Name,
                                  EmploymentStatus = k == null ? "" : k.EmploymentStatus,
                                  IncomeCountry = k == null ? "" : c.Name,
                                  IncomeSource = k == null ? "" : k.IncomeSource,
                                  IncomeState = k == null ? "" : s.Name,
                                  Industry = k == null ? "" : k.Industry,
                                  Occupation = k == null ? "" : k.Occupation,
                                  OccupationDescription = k == null ? "" : k.OccupationDescription,
                                  OwnershipPercentage = k == null ? "" : k.OwnershipPercentage,
                                  Role = k == null ? "" : k.Role,
                                  SourceOfFunds = k == null ? "" : k.SourceOfFund,
                                  Title = k == null ? "" : k.Title,
                                  WealthSource = k == null ? "" : k.WealthSource,
                                  WealthSourceDescription = k == null ? "" : k.WealthSourceDescription
                              },
                              IdentityDocument = new IdentityDocumentModel
                              {
                                  IdentityDocumentBack = k == null ? "" : k.IdentityDocumentBack,
                                  IdentityDocumentFront = k == null ? "" : k.IdentityDocumentFront,
                                  IdentityExpiryDate = k == null ? "" : k.IdentityExpiryDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  IdentityIssueDate = k == null ? "" : k.IdentityIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  IdentityNumber = k == null ? "" : k.IdentityNumber,
                                  IdentityType = k == null ? "" : k.IdentityType
                              }
                          }).FirstOrDefaultAsync();
        }

        public async Task<AccountDetailsModel> GetDetailByUserAsync(long userId)
        {
            return await (from a in _context.Accounts.AsNoTracking()
                          from u in _context.Users.AsNoTracking().Where(u => u.Id == a.UserId).DefaultIfEmpty()
                          from k in _context.Kycs.AsNoTracking().Where(k => k.UserId == u.Id).DefaultIfEmpty()
                          from c in _context.Countries.AsNoTracking().Where(c => c.Code == k.Country).DefaultIfEmpty()
                          from s in _context.States.AsNoTracking().Where(s => s.Code == k.ResidentialState && s.CountryCode == k.Country).DefaultIfEmpty()
                          where a.UserId == userId
                          select new AccountDetailsModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                              EmailAddress = u == null ? "" : u.EmailAddress,
                              Type = k == null ? "" : k.Type,
                              CreatedAt = a == null ? "" : a.CreatedAt.ToString("yyyy-MM-dd"),
                              UserId = u.Id,
                              PersonalInformation = new PersonalInformationModel
                              {
                                  PhoneNumber = u == null ? "" : u.PhoneNumber,
                                  BankStatement = k == null ? "" : k.BankStatement,
                                  CitizenShip = k == null ? "" : c.Name,
                                  City = k == null ? "" : k.City,
                                  DateOfBirth = k == null ? "" : k.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  Line1 = k == null ? "" : k.Address1,
                                  Line2 = k == null ? "" : k.Address2,
                                  State = k == null ? "" : s.Name,
                                  Status = k == null ? "" : k.Status,
                                  TaxDocuments = k == null ? "" : k.TaxDocument,
                              },
                              EmploymentInformation = new EmploymentInformationModel
                              {
                                  AnnualIncome = k == null ? "" : k.AnnualIncome,
                                  Employer = k == null ? "" : k.Employer,
                                  EmployerCountry = k == null ? "" : c.Name,
                                  EmployerState = k == null ? "" : s.Name,
                                  EmploymentStatus = k == null ? "" : k.EmploymentStatus,
                                  IncomeCountry = k == null ? "" : c.Name,
                                  IncomeSource = k == null ? "" : k.IncomeSource,
                                  IncomeState = k == null ? "" : s.Name,
                                  Industry = k == null ? "" : k.Industry,
                                  Occupation = k == null ? "" : k.Occupation,
                                  OccupationDescription = k == null ? "" : k.OccupationDescription,
                                  OwnershipPercentage = k == null ? "" : k.OwnershipPercentage,
                                  Role = k == null ? "" : k.Role,
                                  SourceOfFunds = k == null ? "" : k.SourceOfFund,
                                  Title = k == null ? "" : k.Title,
                                  WealthSource = k == null ? "" : k.WealthSource,
                                  WealthSourceDescription = k == null ? "" : k.WealthSourceDescription
                              },
                              IdentityDocument = new IdentityDocumentModel
                              {
                                  IdentityDocumentBack = k == null ? "" : k.IdentityDocumentBack,
                                  IdentityDocumentFront = k == null ? "" : k.IdentityDocumentFront,
                                  IdentityExpiryDate = k == null ? "" : k.IdentityExpiryDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  IdentityIssueDate = k == null ? "" : k.IdentityIssueDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                                  IdentityNumber = k == null ? "" : k.IdentityNumber,
                                  IdentityType = k == null ? "" : k.IdentityType
                              }
                          }).FirstOrDefaultAsync();
        }
    }
}

