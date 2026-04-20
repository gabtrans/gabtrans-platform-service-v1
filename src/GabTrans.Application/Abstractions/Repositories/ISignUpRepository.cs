using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ISignUpRepository
    {
        Task<List<Role>> GetRolesAsync();
        Task<Role> GetRoleAsync(string name);
        Task<bool> UpdateAsync(UserRole userRole);
        Task<List<IdNameObject>> GetAllRolesAsync();
        Task<bool> AddIndustryAsync(string name);
        Task<User> GetUserIdAsync(long userId);
        Task<UserRole> GetUserRoleAsync(long userId);
        Task<User> UseEmailAddressAsync(string emailAddress);
        Task<Kyc> GetKycByUserIdAsync(long userId);
        Task<User> GetPhoneAsync(string phoneNumber);
        Task<List<IdNameObject>> GetIdentificationsAsync();
        Task<List<IdNameObject>> GetIndustriesAsync();
        Task<List<IdNameObject>> GetSourceOfFundsAsync();
        Task<List<IdNameObject>> GetOccupationsAsync();
        Task<List<IdNameObject>> GetEmploymentStatusesAsync();
        Task<User> GetPhoneUserIdAsync(string phoneNumber, long userId);
        Task<long> CreateAsync(string emailAddress, string firstName, string lastName, string type, long roleId);
    }
}
