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
    public class ChannelRepository : IChannelRepository
    {
        private readonly GabTransContext _context;

        public ChannelRepository(GabTransContext context)
        {
            _context = context;
        }

        public async Task<Channel> DetailsAsync(string name)
        {
            return await _context.Channels.Where(x => x.Name == name).FirstOrDefaultAsync();
        }
    }
}
