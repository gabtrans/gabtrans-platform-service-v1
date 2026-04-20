using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GabTrans.Infrastructure.Repositories
{
    public class SignUpRepository(GabTransContext context, ILogService logService) : ISignUpRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleAsync(string name)
        {
            return await _context.Roles.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<IdNameObject>> GetAllRolesAsync()
        {
            return await _context.Roles.AsNoTracking().Select(x => new IdNameObject { Id = x.Id, Description = x.Name, Name = x.Name }).ToListAsync();
        }

        public async Task<Kyc> GetKycByUserIdAsync(long userId)
        {
            return await _context.Kycs.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> GetUserIdAsync(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> GetPhoneUserIdAsync(string phoneNumber, long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Id == userId);
        }

        public async Task<User> GetPhoneAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<User> UseEmailAddressAsync(string emailAddress)
        {
            return await _context.Users.Where(x => x.EmailAddress == emailAddress).FirstOrDefaultAsync();
        }

        public async Task<UserRole> GetUserRoleAsync(long userId)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<long> CreateAsync(string emailAddress, string firstName, string lastName, string type, long roleId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new User
                {
                    CreatedAt = DateTime.Now,
                    EmailAddress = emailAddress,
                    FirstName = firstName,
                    LastName = lastName
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var kyc = new Kyc
                {
                    UserId = user.Id,
                    Status = KycStatuses.Pending,
                };

                if (!string.IsNullOrEmpty(type)) kyc.Type = type;

                _context.Kycs.Add(kyc);
                await _context.SaveChangesAsync();

                var userRole = new UserRole
                {
                    RoleId = roleId,
                    UserId = user.Id
                };
                _context.UserRoles.Add(userRole);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                _logService.LogError("SignUpRepository", "CreateAsync", ex);
                await transaction.RollbackAsync();
            }
            return 0;
        }

        public async Task<List<IdNameObject>> GetIdentificationsAsync()
        {
            return await _context.IdentityTypes.OrderBy(x => x.Name).Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<List<IdNameObject>> GetIndustriesAsync()
        {
            return await _context.Industries.OrderBy(x => x.Name).Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<List<IdNameObject>> GetOccupationsAsync()
        {
            return await _context.Occupations.OrderBy(x => x.Name).Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<List<IdNameObject>> GetEmploymentStatusesAsync()
        {
            return await _context.EmploymentStatuses.OrderBy(x => x.Name).Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<bool> AddIndustryAsync(string name)
        {

            try
            {
                var user = new Industry
                {
                    Name = name
                };
                _context.Industries.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("SignUpRepository", "CreateAsync", ex);
            }
            return false;
        }

        public async Task<List<IdNameObject>> GetSourceOfFundsAsync()
        {
            return await _context.SourceOfFunds.OrderBy(x => x.Name).Select(x => new IdNameObject { Id = x.Id, Name = x.Name, Description = x.Name }).ToListAsync();
        }

        public async Task<bool> UpdateAsync(UserRole userRole)
        {
            try
            {
                _context.UserRoles.Update(userRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("SignUpRepository", "UpdateAsync", ex);
            }

            return false;
        }
    }
}
