using Cinemax.Scheduler.Jobs;
using Cinemax.Scheduler.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Cinemax.Scheduler.Quartz
{
    public static class QuartzService
    {
        public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true;
                options.Scheduling.OverWriteExistingData = false;
            });

            _ = services.AddQuartz(async quartz =>
            {
                quartz.UseMicrosoftDependencyInjectionJobFactory();

                foreach (var schedulerItem in new List<(string SchedulerName, Type JobType)>
                {
                    ("UpdateScreeningsStatusCron", typeof(UpdateScreeningsStatusJob)),
                    ("UpdateOrdersStatusCron", typeof(UpdateOrdersStatusJob)),
                    ("SendEmailNotificationsCron", typeof(SendEmailNotificationsJob))
                })
                {
                    var cron = configuration.GetSection(SchedulerSettings.SectionName)?[schedulerItem.SchedulerName];
                    if (!string.IsNullOrEmpty(cron) && !cron.Contains("off"))
                    {
                        var jobDataMap = new JobDataMap();
                        var jobKey = new JobKey(Guid.NewGuid().ToString());

                        quartz.AddJob(schedulerItem.JobType, jobKey, options =>
                        {
                            options.UsingJobData(jobDataMap).Build();
                        });

                        quartz.AddTrigger(options =>
                        {
                            options.ForJob(jobKey);
                            options.WithIdentity(Guid.NewGuid().ToString());
                            if (cron.Contains("now"))
                                options.StartNow();
                            else
                                options.WithCronSchedule(cron);
                        });
                    }
                }
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            return services;
        }
    }
}
