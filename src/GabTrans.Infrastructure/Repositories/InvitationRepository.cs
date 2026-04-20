using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class InvitationRepository(GabTransContext context, ILogService logService) : IInvitationRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<Invitation> GetByTokenAsync(string token)
        {
            return await _context.Invitations.Where(x => x.SecretToken == token).FirstOrDefaultAsync();
        }

        public async Task<Invitation> GetAsync(string emailAddress)
        {
            return await _context.Invitations.Where(x => x.EmailAddress == emailAddress).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(Invitation invitation)
        {
            try
            {
               _context.Invitations.Add(invitation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InvitationRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Invitation invitation)
        {
            try
            {
                _context.Invitations.Update(invitation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(InvitationRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
