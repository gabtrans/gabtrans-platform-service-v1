using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Infrastructure.Data;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace GabTrans.Infrastructure.Repositories
{
    public class WebhookRepository(GabTransContext context, ILogService logService) : IWebhookRepository
    {
        private readonly GabTransContext _context = context;
        private readonly ILogService _logService = logService;

        public async Task<bool> InsertAsync(WebHook webHook)
        {
            try
            {
                _context.WebHooks.Add(webHook);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(WebhookRepository), nameof(InsertAsync), ex);
            }

            return false;
        }

        public async Task<bool> UpdateAsync(WebHook webHook)
        {
            try
            {
                _context.WebHooks.Update(webHook);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(nameof(WebhookRepository), nameof(UpdateAsync), ex);
            }

            return false;
        }
    }
}
