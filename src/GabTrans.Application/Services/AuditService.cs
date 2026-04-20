using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class AuditService(IAuditRepository auditRepository) : IAuditService
    {
        public readonly IAuditRepository _auditRepository = auditRepository;

        public async Task InsertAsync(long actorId, long activity, string browser, string ipAddress, string? description = null)
        {
            var audit = new Audit
            {
                CreatedAt = DateTime.Now,
                ModuleActionId = activity,
                ChannelId = (long)Channels.Web,
                UserId = actorId,
                Browser = browser,
                IpAddress = ipAddress,
                Description = description
            };

            await _auditRepository.InsertAsync(audit);
        }
    }
}
