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
    public class BusinessTeamRepository(GabTransContext context, ILogService logService) : IBusinessTeamRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<List<BusinessTeam>> GetAsync(long businessId)
        {
            return await _context.BusinessTeams.Where(x => x.BusinessId == businessId).ToListAsync();
        }

        public async Task<BusinessTeam> GetByIdAsync(long id)
        {
            return await _context.BusinessTeams.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<BusinessTeam> GetByUserIdAsync(long userId)
        {
            return await _context.BusinessTeams.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertAsync(BusinessTeam businessTeam)
        {
            try
            {
                _context.BusinessTeams.Add(businessTeam);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(BusinessTeamRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(BusinessTeam businessTeam)
        {
            try
            {
                _context.BusinessTeams.Update(businessTeam);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(BusinessTeamRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
