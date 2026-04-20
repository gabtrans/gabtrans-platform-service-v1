using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class SignInRepository:ISignInRepository
    {
        private readonly GabTransContext _context;


        public SignInRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAttempts(long userId)
        {
            var login =await _context.Logins.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            login.Attempts = 0;
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateLoginDateAsync(long userId, string deviceId)
        {
            var model = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            model.LastLogin = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            if (!string.IsNullOrEmpty(deviceId)) model.DeviceId = deviceId;

            var login = await _context.Logins.Where(x => x.UserId == userId).FirstOrDefaultAsync();
           // login.IsUserLogin = true;
            await _context.SaveChangesAsync();
            return model.Id > 0;
        }
    }
}
