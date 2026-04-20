using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Models;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;

namespace GabTrans.Application.Services
{
    public class VirtualAccountService : IVirtualAccountService
    {
        private readonly ILogService _logService;
        private readonly ISequenceService _sequenceService;
        private readonly IAccountRepository _accountRepository;
        private readonly IVirtualAccountRepository _virtualAccountRepository;
        public VirtualAccountService(ILogService logService, ISequenceService sequenceService, IAccountRepository accountRepository, IVirtualAccountRepository virtualAccountRepository)
        {
            _logService = logService;
            _sequenceService=sequenceService;
            _accountRepository=accountRepository;
            _virtualAccountRepository = virtualAccountRepository;
        }

        public async Task<bool> CreateAsync(long accountId, string bankName, string accountNumber, string accountName, string routingCode, long paymentTypeId, bool isPrimary)
        {
            try
            {
                //return await _virtualAccountRepository.CreateAsync(accountId, bankName, accountNumber, accountName, routingCode, paymentTypeId, isPrimary);
            }
            catch (Exception ex)
            {
                _logService.LogError("VirtualAccountService", "Create", ex);
            }
            return false;
        }

        public async Task<VirtualAccount> DetailsByAccountAsync(long accountId)
        {
            return await _virtualAccountRepository.GetAsync(accountId);
        }

        public async Task<VirtualAccount> DetailsByBankAsync(long accountId, string bankName)
        {
            return await _virtualAccountRepository.DetailsByBankAsync(accountId, bankName);
        }

        public async Task GenerateAsync()
        {
            try
            {
                var accounts = await _accountRepository.WithoutVirtualAsync();
                foreach (var account in accounts)
                {
                   // if (StaticData.Domain==Enum.GetName(Domains.Test).ToLower()) await GenerateMockAsync(account);
                    //route by domain and country code
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("VirtualAccountService", "GenerateAsync", ex);
            }
        }

        public async Task GenerateMockAsync(Account account)
        {
            long looper = 10;

            var accountDetails = await _virtualAccountRepository.GetAsync(account.Id);
            if (accountDetails is not null) return;

            for (long i = 0; i <= looper; i++)
            {
                string accountNumber = _sequenceService.GenerateAccountNumber();
                var details = await _virtualAccountRepository.DetailsByNumberAsync(accountNumber);
                if (details is null)
                {
                   // await _virtualAccountRepository.CreateAsync(account.Id, Enum.GetName(Banks.Buderless), accountNumber, account.Name, string.Empty, PaymentTypes.Local, true);
                    break;
                }
            }
        }

        public async Task<BankObject> GetAccountAsync(long accountId, string currencyCode)
        {
            try
            {
                //var dd = await _context.Virtualaccounts.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
                //var ss = new FundAccountObject
                //{
                //    AccountHolderName = dd.AccountName,
                //    AccountNumber = dd.AccountNumber,
                //    BankName = dd.Bank.Name,
                //    BankCountry = dd.Bank.CountryCode,
                //    Currency = currency,
                //    PaymentType = dd.PaymentTypeId,
                //    PaymentTypeId = dd.PaymentTypeId

                //};

                //var query = await (from v in _context.Virtualaccounts.AsNoTracking()
                //                   from b in _context.Banks.AsNoTracking().Where(b => b.Id == v.BankId).DefaultIfEmpty()
                //                       //from c in _context.Currencies.AsNoTracking().Where(c => c.Code == v..CurrencyCode).DefaultIfEmpty()
                //                   from cc in _context.Countries.AsNoTracking().Where(cc => cc.Code == b.CountryCode).DefaultIfEmpty()
                //                   where a.AccountId.Equals(accountId)
                //                   select new FundAccountObject
                //                   {
                //                       Id = c.Id,
                //                       Code = c.Code,
                //                       Flag = cc.Flag,
                //                       Name = cc.CurrencyName,
                //                       Symbol = c.Symbol,
                //                       Balance = a.CurrentBalance.ToString("N2")
                //                   }).OrderBy(x => x.Code).ToListAsync();
                //return query;
                //var account = await _context.Accounts.Where(x => x.Id == accountId).FirstOrDefaultAsync();
                //if (account == null) return null;

                //var vaccount = await _context.Virtualaccounts.Where(x => x.AccountId == accountId && x.IsPrimary).FirstOrDefaultAsync();
                //if (vaccount == null) return null;

                //var kyc = await _context.Kycs.Where(x => x.UserId == vaccount.).FirstOrDefaultAsync();
                //if (kyc == null) return null;

                //var bank = await _context.Banks.Where(x => x.Id == vaccount.BankId).FirstOrDefaultAsync();
                //if (bank == null) return null;

                //var country = await _context.Countries.Where(x => x.Code == kyc.CountryCode).FirstOrDefaultAsync();
                //if (country == null) return null;

                //var wallet = await _context.Accountwallets.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
                //if (wallet == null) return null;

                //return new BankObject
                //{
                //    AccountName = account.Name,
                //    AccountNumber = vaccount.AccountNumber,
                //    AccountType = vaccount.AccountType,
                //    BankName = bank.Name,
                //    CurrencyCode = country.Currency,
                //    SortCode = vaccount.RoutingCode,
                //    CurrentBalance = wallet.CurrentBalance
                //};
            }
            catch (Exception ex)
            {
                _logService.LogError("VirtualAccountService", "GetAccount", ex);
            }
            return new BankObject();
        }
    }
}
