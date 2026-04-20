using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IDisputeRepository
    {
        Task<bool> InsertAsync(Dispute dispute);
        Task<bool> UpdateAsync(Dispute dispute);
        Task<Dispute> DetailsAsync(long id);
        Task<Dispute> DetailsAsync(string reference);
        Task<List<DisputeModel>> GetAsync(string reference);
        Task<List<DisputeModel>> GetAsync(GetDisputeRequest request);
    }
}
