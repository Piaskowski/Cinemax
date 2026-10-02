
namespace Cinemax.Scheduler.Settings
{
    public class SchedulerSettings
    {
        public static readonly string SectionName = "Scheduler";
        public string? UpdateScreeningsStatusCron { get; set; }
        public string? UpdateOrdersStatusCron { get; set; }
    }
}
