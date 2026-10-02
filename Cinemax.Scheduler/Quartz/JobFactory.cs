
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Cinemax.Scheduler.Quartz
{
    public class JobFactory(IServiceProvider serviceProvider) : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            => (_serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob) ?? throw new Exception("NewJob not created");

        public void ReturnJob(IJob job)
            => (job as IDisposable)?.Dispose();
    }
}
