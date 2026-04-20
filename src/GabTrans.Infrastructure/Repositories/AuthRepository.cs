using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Logging;

namespace GabTrans.Infrastructure.Repositories
{
    public class AuthRepository(GabTransContext context, ILogService logService) : IAuthRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(AuthCredential authCredential)
        {
            try
            {
                _context.AuthCredentials.Add(authCredential);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(AuthRepository),nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<AuthCredential> DetailsAsync(long id)
        {
            return await _context.AuthCredentials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ValidateAsync(string token)
        {
            return await _context.AuthCredentials.AnyAsync(x => x.Token == token && x.Status=="active");
        }

        public async Task<bool> ValidateAsync(string username, string password)
        {
            return await _context.AuthCredentials.AnyAsync(x => x.AppId == username && x.AppKey == password && x.Status == "active");
        }
    }
}
