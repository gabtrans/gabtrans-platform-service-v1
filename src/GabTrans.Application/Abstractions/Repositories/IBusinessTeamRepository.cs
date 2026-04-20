using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IBusinessTeamRepository
    {
        Task<bool> InsertAsync(BusinessTeam businessTeam);
        Task<bool> UpdateAsync(BusinessTeam businessTeam);
        Task<BusinessTeam> GetByIdAsync(long id);
        Task<BusinessTeam> GetByUserIdAsync(long userId);
        Task<List<BusinessTeam>> GetAsync(long businessId);
    }
}
