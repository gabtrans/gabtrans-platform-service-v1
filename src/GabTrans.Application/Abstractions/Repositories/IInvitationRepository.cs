using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IInvitationRepository
    {
        Task<bool> InsertAsync(Invitation invitation);
        Task<bool> UpdateAsync(Invitation invitation);
        Task<Invitation> GetAsync(string emailAddress);
        Task<Invitation> GetByTokenAsync(string token);
    }
}
