using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAsync();
        Task<UserDetails> DetailsAsync(string userName);
        Task<UserDetails> GetDetailsByIdAsync(long userId);
        Task<User> GetDetailsByUserIdAsync(long userId);
        Task<User> GetDetailsByPhoneAsync(string phoneNumber);
        Task<User> GetDetailsByUserEmailAsync(string emailAddress);
        Task<List<UserDetails>> GetAsync(QueryUser queryUser);
        Task<bool> UpdatePasswordAsync(long userId, string oldPassword, string password);
        Task<bool> UserAccessAsync(bool isAccountLock, long userId);
        Task<bool> UserLoginDateAsync(long userId);
        Task<bool> UpdateClientIdAsync(long userId, string clientId);
        Task<bool> EnableAsync(long userId);
        Task<bool> DisableAsync(long userId);
        Task<long> InsertAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<User> GetByAccountIdAsync(long accountId);
    }
}

