using GabTrans.Application.Abstractions.Logging;
using GabTrans.Domain.Models;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;

namespace GabTrans.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;

        public UserService(ILogService logService, IUserRepository userRepository)
        {
            _logService = logService;
            _userRepository = userRepository;
        }

        public async Task<bool> UpdatePasswordAsync(long userId, string oldPassword, string password)
        {
            try
            {
                return await _userRepository.UpdatePasswordAsync(userId, oldPassword, password);
            }
            catch (Exception ex)
            {
                _logService.LogError("UserService", "UpdatePassword", ex);
            }
            return false;
        }

        public async Task<User> GetDetailsByUserIdAsync(long userId)
        {
            return await _userRepository.GetDetailsByUserIdAsync(userId);
        }

        public async Task<User> GetDetailsByUserEmailAsync(string emailAddress)
        {
            return await _userRepository.GetDetailsByUserEmailAsync(emailAddress);
        }
    }
}

