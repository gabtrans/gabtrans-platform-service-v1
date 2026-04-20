using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface ITradeCryptoService
    {
        Task<ApiResponse> CreateAsync(TradeCryptoRequest request, long userId);
        Task<PaginatedResponse> GetAsync(QueryTransaction queryTransaction);
    }
}
