using Microsoft.EntityFrameworkCore;
using System.Data;
using GabTrans.Infrastructure.Data;
using GabTrans.Domain.Entities;
using GabTrans.Application.Abstractions.Repositories;

namespace GabTrans.Infrastructure.Repositories
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly GabTransContext _context;
        public SequenceRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<SerialNumber> DetailsAsync(long id)
        {
            return await _context.SerialNumbers.Where(x=>x.Id==id).FirstOrDefaultAsync();
        }

        public async Task<List<SerialNumber>> GetAsync()
        {
            return await _context.SerialNumbers.ToListAsync();
        }

        public async Task<bool> UpdateAsync(long id, long lastCount)
        {
            var serialNumber= await _context.SerialNumbers.Where(x => x.Id == id).FirstOrDefaultAsync();
            serialNumber.LastCount = lastCount;
            serialNumber.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return serialNumber.Id > 0;
        }

        public async Task<bool> AssignNumberAsync(long userId, string customerNumber)
        {
            var kyc = _context.Kycs.Where(x => x.UserId == userId).FirstOrDefault();
          //  kyc.CustomerNumber = customerNumber;
            //kyc.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return kyc.Id > 0;
        }
    }
}
