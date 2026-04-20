using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Infrastructure.Repositories
{
    public class TransferProviderRepository(GabTransContext context) : ITransferProviderRepository
    {
        private readonly GabTransContext _context = context;

        public async Task<TransferProvider> GetByCurrency(string currency)
        {
           return await _context.TransferProviders.AsNoTracking().Where(x=>x.Currency == currency).FirstOrDefaultAsync();
        }
    }
}
