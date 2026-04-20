using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace GabTrans.Infrastructure.Repositories
{
    public class SettlementRepository(GabTransContext context, ILogService logService) : ISettlementRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<decimal> DailyCumulativeAsync(long accountId)
        {
            return await _context.Settlements.Where(x => x.AccountId == accountId && x.CreatedAt >= DateTime.Now.Date).SumAsync(x => x.Amount);
        }

        public async Task<Settlement> DetailsAsync(string referenceNumber)
        {
            return await _context.Settlements.AsNoTracking().Where(x => x.Reference == referenceNumber).FirstOrDefaultAsync();
        }

        public async Task<Settlement> DetailsAsync(string referenceNumber, string indicator)
        {
            return await _context.Settlements.AsNoTracking().Where(x => x.Reference == referenceNumber && x.DebitCreditIndicator == indicator).FirstOrDefaultAsync();
        }

        public async Task<Settlement> DetailsAsync(long id)
        {
            return await _context.Settlements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Settlement>> GetAsync(GetTransactionHistoryRequest request)
        {
            DateTime startDate = string.IsNullOrEmpty(request.StartDate) ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(request.StartDate);
            DateTime endDate = string.IsNullOrEmpty(request.EndDate) ? DateTime.Now.AddDays(1) : Convert.ToDateTime(request.EndDate);

            return await _context.Settlements.AsNoTracking().Where(w => w.AccountId == request.AccountId && w.CreatedAt >= startDate && w.CreatedAt < endDate && (request.Category == "")).OrderByDescending(x => x.Id).ToListAsync();
        }

        //public async Task<string> ProcessAsync(SettlementModel settlementModel)
        //{
        //    string? walletStatus = null;

        //    try
        //    {
        //        _logService.LogInfo("SettlementRepository", "Started wallet transaction for reference ::", settlementModel.Reference);

        //        var mySqlParameters = new[]
        //        {
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_WalletId", NpgsqlDbType =  NpgsqlDbType.Bigint,
        //         Value = settlementModel.WalletId,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_TransactionAmount", NpgsqlDbType = NpgsqlDbType.Numeric,
        //         Value = settlementModel.TransactionAmount,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_TransactionFee", NpgsqlDbType = NpgsqlDbType.Numeric,
        //         Value = settlementModel.TransactionFee,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_Currency", NpgsqlDbType = NpgsqlDbType.Varchar,
        //         Value = settlementModel.Currency,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_Indicator", NpgsqlDbType = NpgsqlDbType.Varchar,
        //         Value = settlementModel.DebitCreditIndicator,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_AccountId", NpgsqlDbType = NpgsqlDbType.Bigint,
        //         Value = settlementModel.AccountId,
        //         Direction = ParameterDirection.Input
        //     },
        //     new NpgsqlParameter
        //     {
        //         ParameterName = "@p_Reference", NpgsqlDbType = NpgsqlDbType.Varchar,
        //         Value = settlementModel.Reference,
        //         Direction = ParameterDirection.Input
        //     },
        //            new NpgsqlParameter
        //     {
        //         ParameterName = "@p_TransactionType", NpgsqlDbType = NpgsqlDbType.Varchar,
        //         Value = settlementModel.TransactionType,
        //         Direction = ParameterDirection.Input
        //     },
        //       new NpgsqlParameter
        //     {
        //         ParameterName = "@p_Note", NpgsqlDbType = NpgsqlDbType.Varchar,
        //         Value = settlementModel.Note,
        //         Direction = ParameterDirection.Input
        //     }
        //    //,
        //    //   new NpgsqlParameter
        //    // {
        //    //     ParameterName = "@p_WalletStatus", NpgsqlDbType = NpgsqlDbType.Varchar,
        //    //     Direction = ParameterDirection.Output
        //    // }
        // };

        //        await using var command = _context.Database.GetDbConnection().CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "sp_ProcessSettlement";
        //        command.Parameters.AddRange(mySqlParameters);
        //        await _context.Database.OpenConnectionAsync();

        //        await command.ExecuteNonQueryAsync();

        //        walletStatus = command.Parameters["@p_WalletStatus"].Value == null ? null : (string?)command.Parameters["@p_WalletStatus"].Value;
        //    }
        //    catch (Exception e)
        //    {
        //        _logService.LogError("SettlementRepository", "An error occurred ProcessWalletTransaction for reference : " + settlementModel.Reference + " ::: ", e);
        //    }
        //    _logService.LogInfo("SettlementRepository", "Final  wallet transaction status for reference ::" + settlementModel.Reference + " is :: ", walletStatus);
        //    return walletStatus;
        //}

        public async Task<string> ProcessAsync(SettlementModel settlementModel)
        {
            var result = new object();

            try
            {
                const string sql = "SELECT gabtrans.sp_process_settlement(" +
                                       "@account_id, @wallet_id, @transaction_amount, @transaction_fee, " +
                                       "@currency, @indicator, @reference, @note, @transaction_type)";

                await using var connection = new NpgsqlConnection(StaticData.ConnectionStrings);
                await connection.OpenAsync();

                await using var cmd = new NpgsqlCommand(sql, connection);

                cmd.Parameters.Add(new NpgsqlParameter("@account_id", NpgsqlDbType.Bigint)
                { Value = settlementModel.AccountId });

                cmd.Parameters.Add(new NpgsqlParameter("@wallet_id", NpgsqlDbType.Bigint)
                { Value = settlementModel.WalletId });

                cmd.Parameters.Add(new NpgsqlParameter("@transaction_amount", NpgsqlDbType.Numeric)
                { Value = settlementModel.TransactionAmount });

                cmd.Parameters.Add(new NpgsqlParameter("@transaction_fee", NpgsqlDbType.Numeric)
                { Value = settlementModel.TransactionFee });

                cmd.Parameters.Add(new NpgsqlParameter("@currency", NpgsqlDbType.Varchar)
                { Value = settlementModel.Currency });

                cmd.Parameters.Add(new NpgsqlParameter("@indicator", NpgsqlDbType.Varchar)
                { Value = settlementModel.DebitCreditIndicator }); // "debit" or "credit"

                cmd.Parameters.Add(new NpgsqlParameter("@reference", NpgsqlDbType.Varchar)
                { Value = settlementModel.Reference });

                cmd.Parameters.Add(new NpgsqlParameter("@note", NpgsqlDbType.Varchar)
                { Value = (object?)settlementModel.Note ?? DBNull.Value });

                cmd.Parameters.Add(new NpgsqlParameter("@transaction_type", NpgsqlDbType.Varchar)
                { Value = settlementModel.TransactionType });

               result = await cmd.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                _logService.LogError("SettlementRepository", "An error occurred ProcessWalletTransaction for reference : " + settlementModel.Reference + " ::: ", e);
            }
            _logService.LogInfo("SettlementRepository", "Final  wallet transaction status for reference ::" + settlementModel.Reference + " is :: ", result!.ToString());
            return result?.ToString() ?? "error_unknown";
        }

        public async Task<long> GetTotalTransactionAsync()
        {
            return await _context.Settlements.Where(s => s.Type != TransactionTypes.Charges).CountAsync();
        }
    }
}
