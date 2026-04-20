using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IAuditRepository
    {
        Task<bool> UpdateAsync(Audit audit);
        Task InsertAsync(Audit audit);
        Task<List<AuditDetails>> GetAsync(long userId);
        Task InsertAsync(long actorId, long activity, long channelId, string browser, string? ipAddress = null, string? description = null);
        Task<List<AuditDetails>> GetAsync(GetAuditRequest request);
    }
}

