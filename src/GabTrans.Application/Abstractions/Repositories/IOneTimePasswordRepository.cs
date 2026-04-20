using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IOneTimePasswordRepository
    {
        Task<OneTimePassword> DetailsAsync(string password);
        Task<bool> CreateAsync(long userId, string token, long categoryId);
        Task<OneTimePassword> DetailsAsync(long userId, string password);
        Task<OneTimePassword> DetailsAsync(string password, long categoryId);
        Task<bool> UpdateAsync(long id, bool isUsed);
        Task<bool> UpdateAsync(OneTimePassword oneTimePassword);
        Task<bool> UpdateKycAsync(long userId, long categoryId);
        Task<bool> DeactivateAsync(long userId, long otpCategoryId);
        Task<OneTimePassword> GetUnusedPasswordAsync(long userId, long categoryId);
    }
}
