using GabTrans.Domain.Entities;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ITwoFactorAuthRepository
    {
        Task<bool> SaveToken(long userId, string uniqueToken);
        Task<UserToken> Details(long userId);
        Task<UserToken> Details(long userId, string uniqueToken);    
    }
}
