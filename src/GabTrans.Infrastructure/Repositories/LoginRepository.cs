using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class LoginRepository(GabTransContext dbContext, ILogService logService) : ILoginRepository
    {
        private readonly GabTransContext _dbContext = dbContext;
        private readonly ILogService _logService = logService;

        public async Task<long> GetFailedAttemptAsync(long userId)
        {
            var login = await _dbContext.Logins.FirstOrDefaultAsync(x => x.UserId == userId);
            return login == null ? 0 : login.Attempts;
        }

        public async Task<bool> InsertAsync(Login login)
        {
            try
            {
                _dbContext.Logins.Add(login);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LoginRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(long userId)
        {
            try
            {
                var login = await _dbContext.Logins.FirstOrDefaultAsync(x => x.UserId == userId);
                if (login is null) return false;

                _dbContext.Logins.Remove(login);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LoginRepository), nameof(DeleteAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(Login login)
        {
            try
            {
                _dbContext.Logins.Update(login);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LoginRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<Login> DetailsAsync(long userId)
        {
            return await _dbContext.Logins.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Login> ValidateTokenAsync(string sessionToken)
        {
            return await _dbContext.Logins.AsNoTracking().Where(x => x.SessionToken == sessionToken).FirstOrDefaultAsync();
        }

        public async Task<Login> ValidateTokenAsync(string sessionToken, string refereshToken)
        {
            return await _dbContext.Logins.AsNoTracking().Where(x => x.SessionToken == sessionToken && x.RefreshToken == refereshToken).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync(LoginModel loginModel, string ipAddress)
        {
            try
            {
                var login = await _dbContext.Logins.Where(x => x.UserId == loginModel.UserId).FirstOrDefaultAsync();
                if (login is null)
                {
                    _dbContext.Logins.Add(new Login
                    {
                        CreatedAt = DateTime.Now,
                        RefreshToken = loginModel.RefreshToken,
                        RefreshTokenExpiryTime = loginModel.RefreshTokenExpiryTime,
                        LastAccessed = DateTime.Now,
                        IpAddress = ipAddress,
                        UserId = loginModel.UserId,
                        SessionToken = loginModel.SessionToken,
                        Attempts = 1,
                        Status = loginModel.Status
                    });
                }
                else
                {
                    login.RefreshToken = loginModel.RefreshToken;
                    login.RefreshTokenExpiryTime = loginModel.RefreshTokenExpiryTime;
                    login.LastAccessed = DateTime.Now;
                    login.SessionToken = loginModel.SessionToken;
                    login.IpAddress = ipAddress;
                    login.Attempts = login.Attempts + 1;
                    login.Status = loginModel.Status;
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(LoginRepository), nameof(SaveAsync), ex);
            }
            return false;
        }
    }
}
