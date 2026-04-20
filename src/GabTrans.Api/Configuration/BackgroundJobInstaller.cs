using Quartz;
using Quartz.AspNetCore;

namespace GabTrans.Api.Configuration
{
    public class BackgroundJobInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("BackgroundJob");
                q.AddJob<BackgroundJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("BackgroundJob-trigger").WithSimpleSchedule(o => o
                        .RepeatForever()
                        .WithIntervalInSeconds(5))
                );

               // AddJob<SampleJob>(q, "0 * * * * ?"); // every minute
            });

            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }
    }
}
