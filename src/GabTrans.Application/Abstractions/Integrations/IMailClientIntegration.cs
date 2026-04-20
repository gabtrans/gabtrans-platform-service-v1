using GabTrans.Application.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Integrations
{
    public interface IMailClientIntegration
    {
        Task<bool> SendAsync(SendMailRequest sendMailRequest);
    }
}
