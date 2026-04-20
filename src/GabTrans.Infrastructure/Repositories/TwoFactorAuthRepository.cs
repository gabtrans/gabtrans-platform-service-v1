using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories
{
    public class TwoFactorAuthRepository : ITwoFactorAuthRepository
    {
        private readonly GabTransContext _context;

        public TwoFactorAuthRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveToken(long userId, string uniqueToken)
        {
            var tokenDetails = await _context.UserTokens.Where(x => x.UserId==userId).FirstOrDefaultAsync();
            if(tokenDetails is null)
            {
                var userToken = new UserToken
                {
                    CreatedAt = DateTime.Now,
                    Active = true,
                    UniqueToken = uniqueToken,
                    UserId = userId
                };

                _context.UserTokens.Add(userToken);
            }
            else
            {
                tokenDetails.UniqueToken=uniqueToken;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserToken> Details(long userId)
        {
            return await _context.UserTokens.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<UserToken> Details(long userId, string uniqueToken)
        {
            return await _context.UserTokens.Where(x => x.UserId == userId && x.UniqueToken == uniqueToken).FirstOrDefaultAsync();
        }       
    }
}
