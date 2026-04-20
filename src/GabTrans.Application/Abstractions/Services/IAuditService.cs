using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IAuditService
    {
        Task InsertAsync(long actorId, long activity, string browser, string ipAddress, string? description = null);
    }
}
