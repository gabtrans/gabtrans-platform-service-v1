using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IDepositRepository
    {
        Task<Deposit> DetailsByReferenceAsync(string referenceNumber);
        Task<Deposit> DetailsByTranRefAsync(string tranReference);
        Task<List<IdNameObject>> GetPaymentsAsync();
        // Task<List<PaymentType>> GetPaymentTypesAsync();
        Task<List<IdNameObject>> GetAccountTypesAsync();
        Task<DepositModel> DetailsAsync(long id);
        Task<List<TransactionHistory>> GetTransactionsAsync(GetTransactionRequest request);
        //Task<Bank> BankDetailsByIdAsync(long bankId);
        //Task<Bank> BankDetailsByNameAsync(string name);
        //Task<Bank> BankDetailsByCodeAsync(string bankCode);
        //  Task<List<BankModel>> GetBanksAsync(string countryCode);
        // Task<bool> IsBankValidAsync(string bankCode);
        Task<long> InsertAsync(Deposit transaction);
        Task<List<IdNameObject>> GetTransactionTypesAsync();
        // Task<List<TransactionStatus>> GetTransactionStatusAsync();
        Task<List<SummaryValue>> RevenuesAsync();
        Task<VirtualAccountObject> GetCustomerAccountAsync(long accountId, string bankName);
        // Task<PaymentType> GetPaymentTypeAsync(long id);
        // Task<List<IdNameCode>> GetBankListAsync(string countryCode);
        Task<List<Deposit>> GetAsync(GetTransactionHistoryRequest request);
        //TransactionDetails DetailsAsync(long id, string referenceNumber);
        Task<bool> UpdateStatusAsync(long id, string status, bool refunded, string? gatewayReference = null);
        Task<bool> UpdateAsync(long id, string status, string responseMessage, string? gatewayResponse, bool refunded, string? tranRef = null);
        Task<List<TransactionModel>> GetAsync(QueryTransaction queryTransaction);
     }
}
