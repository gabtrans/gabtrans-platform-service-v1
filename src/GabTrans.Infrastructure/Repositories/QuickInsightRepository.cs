using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Enums;

namespace GabTrans.Infrastructure.Repositories
{
    public class QuickInsightRepository(GabTransContext context) : IQuickInsightRepository
    {
        private readonly GabTransContext _context = context;

        public async Task<SummaryCount> TotalTransfersAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Transfers
                .Where(t => t.Status == TransactionStatuses.Successful && t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .SumAsync(t => t.Amount);

            // Total for last month
            var lastMonthTotal = await _context.Transfers
                .Where(t => t.Status == TransactionStatuses.Successful && t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .SumAsync(t => t.Amount);

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }

        public async Task<SummaryCount> TotalAccountsAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Accounts
                .Where(t => t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .CountAsync();

            // Total for last month
            var lastMonthTotal = await _context.Accounts
                .Where(t => t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .CountAsync();

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }

        public async Task<SummaryCount> TotalRevenuesAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Settlements
                .Where(t => t.Type == TransactionTypes.Charges && t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .SumAsync(t => t.Amount);

            // Total for last month
            var lastMonthTotal = await _context.Settlements
                .Where(t => t.Type == TransactionTypes.Charges && t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .SumAsync(t => t.Amount);

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }

        public async Task<SummaryCount> TotalPendingKycAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Kycs
                .Where(t => t.Status == KycStatuses.Completed && t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .CountAsync();

            // Total for last month
            var lastMonthTotal = await _context.Kycs
                .Where(t => t.Status == KycStatuses.Completed && t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .CountAsync();

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }

        public async Task<SummaryCount> TotalBalancesAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Wallets
                .Where(t => t.Currency == Currencies.USD && t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .SumAsync(t => t.Balance);

            // Total for last month
            var lastMonthTotal = await _context.Wallets
                .Where(t => t.Currency == Currencies.USD && t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .SumAsync(t => t.Balance);

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }

        public async Task<SummaryCount> OpenTicketAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // Total for current month
            var currentMonthTotal = await _context.Disputes
                .Where(t => t.Status == DisputeStatuses.Open && t.CreatedAt >= currentMonthStart && t.CreatedAt <= now)
                .CountAsync();

            // Total for last month
            var lastMonthTotal = await _context.Disputes
                .Where(t => t.Status == DisputeStatuses.Open && t.CreatedAt >= lastMonthStart && t.CreatedAt <= lastMonthEnd)
                .CountAsync();

            // Percentage change
            var percentageChange = lastMonthTotal == 0 ? 100 : ((currentMonthTotal - lastMonthTotal) / lastMonthTotal) * 100;

            return new SummaryCount
            {
                Value = $"{currentMonthTotal:N2}",
                Percentage = $"{(percentageChange > 0 ? "+" : "")}{percentageChange:N2}% vs last month"
            };
        }
    }
}
