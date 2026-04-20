using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface IDisputeService
    {
        Task<ApiResponse> CreateAsync(CreateDisputeRequest request, long userId, string browser, string ipAddress);
        Task<ApiResponse> UpdateAsync(UpdateDisputeRequest request,long id, long userId, string browser, string ipAddress);
    }
}
