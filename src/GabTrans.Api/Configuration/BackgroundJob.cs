using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.Interfaces.Services;
using Quartz;

namespace GabTrans.Api.Configuration
{
    [DisallowConcurrentExecution]
    public class BackgroundJob(IGRemitService gRemitService) : IJob
    {
        private readonly IGRemitService _gRemitService = gRemitService;

        public async Task Execute(IJobExecutionContext context)
        {
            await _gRemitService.ConfirmationAsync();

            await _gRemitService.ProcessAsync();
        }
    }
}
