using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ISignInRepository
    {
        Task<bool> UpdateLoginDateAsync(long userId, string deviceId);
    }
}
