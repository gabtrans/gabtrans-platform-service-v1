using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Repositories
{
    public interface IWebhookRepository
    {
        Task<bool> InsertAsync(WebHook webHook);
        Task<bool> UpdateAsync(WebHook webHook);
    }
}
