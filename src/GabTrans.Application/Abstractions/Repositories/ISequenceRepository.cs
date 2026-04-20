using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface ISequenceRepository
    {
        Task<List<SerialNumber>> GetAsync();
        Task<SerialNumber> DetailsAsync(long id);
        Task<bool> UpdateAsync(long id, long lastCount);
        Task<bool> AssignNumberAsync(long userId, string customerNumber);
    }
}
