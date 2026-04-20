using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly GabTransContext _context;

        public ReceiptRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<Receipt> DetailsAsync(long transactionId)
        {
            return await (from t in _context.Deposits.AsNoTracking()
                          from ts in _context.TransactionTypes.AsNoTracking().Where(ts => ts.Id == t.Id).DefaultIfEmpty()
                         // from ps in _context.PaymentTypes.AsNoTracking().Where(ps => ps.Id == t.PaymentTypeId).DefaultIfEmpty()
                          //from ss in _context.TransactionStatuses.AsNoTracking().Where(ss => ss.Id == t.Status).DefaultIfEmpty()
                         // from bs in _context.Bills.AsNoTracking().Where(bs => bs.Id == t.Id).DefaultIfEmpty()
                          where t.Id.Equals(transactionId)
                          select new Receipt
                          {
                              Amount = t.Amount,
                             // Bill = bs.Name,
                           //   CountryCode = t.CountryCode,
                              Currency = t.Currency,
                           //   BillId = t.BillId,
                           //   DateCreated = t.CreatedAt,
                           //   ExchangeRate = t.ExchangeRate,
                           //   Fee = t.Fee,
                           //   InitialRequest = t.GatewayRequest,
                           //   InitialResponse = t.GatewayResponse,
                           //   Narration = t.Narration,
                           //   PaymentType = ps.Name,
                           //   ReceiverAccountId = t.ReceiverAccountId,
                           //   PaymentTypeId = t.PaymentTypeId,
                           //   ReceiverBank = t.ReceiverBank,
                           ////   Response = t.Response,
                           //  // PaymentOptionId = t.PaymentOptionId,
                           //   ReceiverAccountName = t.ReceiverAccountName,
                           //   ReceiverAccountNumber = t.ReceiverAccountNumber,
                           //   ReceiverCountryCode = t.ReceiverCountryCode,
                           //   ReferenceNumber = t.ReferenceNumber,
                           //  // Request = t.Request,
                           //   ResponseCode = t.ResponseCode,
                           //   ResponseMessage = t.ResponseMessage,
                           //   RoutingCode = t.RoutingCode,
                           //   //RoutingCodeType = t.RoutingCodeType,
                           //   SenderAccountId = t.SenderAccountId,
                           //   SenderAccountName = t.SenderAccountName,
                           //   SenderAccountNumber = t.SenderAccountNumber,
                           //   SenderBank = t.SenderBank,
                           //  // SenderCountryCode = t.SenderCountryCode,
                           //   SenderCurrency = t.SenderCurrency,
                           //   SettledAmount = t.SettledAmount,
                           //   //Status = t.Status,
                           //   TranReference = t.GateWayReference,
                           //   TransactionId = t.Id,
                           //   TransactionStatus = t.Status,
                           //   TransactionType = ts.Name,
                           //   TransactionTypeId = t.TransactionTypeId
                          }).FirstOrDefaultAsync();
        }
    }
}
