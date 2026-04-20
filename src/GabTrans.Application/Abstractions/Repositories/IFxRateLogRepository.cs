using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IFxRateLogRepository
    {
        Task<bool> InsertAsync(FxRateLog fxRateLog);
        Task<bool> UpdateAsync(FxRateLog fxRateLog);
        Task<FxRateLog> GetAsync(string token);
        Task<FxRateLog> GetAsync(long id);
    }
}
