using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GabTrans.Infrastructure.Data;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Enums;

namespace GabTrans.Infrastructure.Repositories
{
    public class UserRepository(GabTransContext context, ILogService logService) : IUserRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService logService = logService;

        public async Task<UserDetails> DetailsAsync(string userName)
        {
            return await (from u in _context.Users.AsNoTracking()
                          from ur in _context.UserRoles.AsNoTracking().Where(x => x.UserId == u.Id).DefaultIfEmpty()
                          from r in _context.Roles.AsNoTracking().Where(r => r.Id == ur.RoleId)

                          where u.PhoneNumber.Equals(userName) || u.EmailAddress.Equals(userName)
                          select new UserDetails
                          {
                              DateRegistered = u.CreatedAt.ToString("MMMM dd, yyyy hh:mm:ss"),
                              EmailAddress = u.EmailAddress,
                              FirstName = u.FirstName,
                              LastLogin = u.LastLogin,
                              LastName = u.LastName,
                              FullName = $"{u.FirstName} {u.LastName}",
                              MiddleName = u.MiddleName,
                              PhoneNumber = u.PhoneNumber,
                              Role = r.Name,
                              Status = u.Status,
                              Id = u.Id
                          }).FirstOrDefaultAsync();
        }

        public async Task<UserDetails> GetDetailsByIdAsync(long userId)
        {
            return await (from u in _context.Users.AsNoTracking()
                          from ur in _context.UserRoles.AsNoTracking().Where(x => x.UserId == u.Id).DefaultIfEmpty()
                          from r in _context.Roles.AsNoTracking().Where(r => r.Id == ur.RoleId)

                          where u.Id.Equals(userId)
                          select new UserDetails
                          {
                              DateRegistered = u.CreatedAt.ToString("MMMM dd, yyyy hh:mm:ss"),
                              EmailAddress = u.EmailAddress,
                              FirstName = u.FirstName,
                              LastLogin = u.LastLogin,
                              LastName = u.LastName,
                              FullName = $"{u.FirstName} {u.LastName}",
                              MiddleName = u.MiddleName,
                              PhoneNumber = u.PhoneNumber,
                              Role = r.Name,
                              Status = u.Status,
                              Id = u.Id
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<UserDetails>> GetAsync(QueryUser queryUser)
        {
            DateTime startDate = string.IsNullOrEmpty(queryUser.StartDate) ? DateTime.Now.AddDays(-60).Date : Convert.ToDateTime(queryUser.StartDate);
            DateTime endDate = string.IsNullOrEmpty(queryUser.EndDate) ? DateTime.Now.Date : Convert.ToDateTime(queryUser.EndDate);

            return await (from u in _context.Users.AsNoTracking()
                          from ur in _context.UserRoles.AsNoTracking().Where(ur => ur.UserId == u.Id).DefaultIfEmpty()
                          from r in _context.Roles.AsNoTracking().Where(r => r.Id == ur.RoleId).DefaultIfEmpty()
                          where ur.RoleId != (long)Roles.Customer && ur.RoleId != (long)Roles.Member &&
                          (string.IsNullOrEmpty(queryUser.Role) || r.Name == queryUser.Role)
                          && (string.IsNullOrEmpty(queryUser.FullName) || queryUser.FullName.Contains(u.FirstName) || queryUser.FullName.Contains(u.LastName))
                          && (string.IsNullOrEmpty(queryUser.Email) || u.EmailAddress == queryUser.Email)
                          && u.CreatedAt.Date >= startDate.Date && u.CreatedAt.Date <= endDate.Date

                          select new UserDetails
                          {
                              DateRegistered = u.CreatedAt.ToString("MMMM dd, yyyy hh:mm:ss"),
                              EmailAddress = u.EmailAddress,
                              FirstName = u.FirstName,
                              LastLogin = u.LastLogin,
                              LastName = u.LastName,
                              FullName = $"{u.FirstName} {u.LastName}",
                              MiddleName = u.MiddleName,
                              PhoneNumber = u.PhoneNumber,
                              Role = r.Name,
                              Status = u.Status,
                              Id = u.Id
                          }).OrderByDescending(u=>u.Id).ToListAsync();
        }

        public async Task<bool> UpdatePasswordAsync(long userId, string oldPassword, string password)
        {
            var result = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            result.Password = password;
            result.OldPassword = oldPassword;
            await _context.SaveChangesAsync();
            return result.Id > 0;
        }

        public async Task<bool> UserAccessAsync(bool isAccountLock, long userId)
        {
            var param = await _context.Users.FindAsync(userId);
            //param.Locked = isAccountLock;
            await _context.SaveChangesAsync();
            return param.Id > 0;
        }

        public async Task<bool> UserLoginDateAsync(long userId)
        {
            var request = await _context.Users.FindAsync(userId);
            request.LastLogin = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            await _context.SaveChangesAsync();
            return request.Id > 0;
        }

        public async Task<long> InsertAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                logService.LogError(nameof(UserRepository), nameof(InsertAsync), ex);
            }

            return 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logService.LogError(nameof(UserRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }

        public async Task<User> GetDetailsByUserIdAsync(long userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetDetailsByUserEmailAsync(string emailAddress)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == emailAddress);
        }

        public async Task<User> GetDetailsByPhoneAsync(string phoneNumber)
        {
            return await _context.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateClientIdAsync(long userId, string clientId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            //user.ClientId=clientId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableAsync(long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            //user.Locked = true;
            // user.Active = false;
            user.LockedAt = DateTime.Now;
            // user.UpdatedAt=DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnableAsync(long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false;

            //user.Locked = false;
            //user.Active = true;
            //user.UpdatedAt=DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByAccountIdAsync(long accountId)
        {
            return await (from u in _context.Users.AsNoTracking()
                          from a in _context.Accounts.AsNoTracking().Where(a => a.UserId == u.Id).DefaultIfEmpty()
                          where a.Id == accountId
                          select u
                          ).FirstOrDefaultAsync();
        }
    }
}

