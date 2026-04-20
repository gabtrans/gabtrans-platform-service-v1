using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IFxRateAuditRepository
    {
        Task<IEnumerable<FxRateAudit>> GetAsync(QueryFxRate queryFxRate);
        Task<bool> InsertAsync(FxRateAudit fxRateAudit);
    }
}
