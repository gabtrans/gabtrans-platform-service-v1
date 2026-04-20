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
    public interface IQuickInsightRepository
    {
        Task<SummaryCount> OpenTicketAsync();
        Task<SummaryCount> TotalRevenuesAsync();
        Task<SummaryCount> TotalPendingKycAsync();
        Task<SummaryCount> TotalBalancesAsync();
        Task<SummaryCount> TotalTransfersAsync();
        Task<SummaryCount> TotalAccountsAsync();
    }
}
