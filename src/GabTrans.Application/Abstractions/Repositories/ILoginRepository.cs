using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ILoginRepository
    {
        Task<bool> InsertAsync(Login login);
        Task<bool> UpdateAsync(Login login);
        Task<bool> DeleteAsync(long userId);
        Task<Login> DetailsAsync(long userId);
        Task<long> GetFailedAttemptAsync(long userId);
        Task<Login> ValidateTokenAsync(string sessionToken);
        Task<Login> ValidateTokenAsync(string sessionToken, string refereshToken);
        Task<bool> SaveAsync(LoginModel loginModel, string ipAddress);
    }
}
