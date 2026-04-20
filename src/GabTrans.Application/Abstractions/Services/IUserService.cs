using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<User> GetDetailsByUserIdAsync(long userId);
        Task<User> GetDetailsByUserEmailAsync(string emailAddress);
        Task<bool> UpdatePasswordAsync(long userId, string oldPassword, string password);
    }
}

