using Cinemax.Application.Elements.Notifications.Commands;
using MediatR;
using Quartz;

namespace Cinemax.Scheduler.Jobs
{
    public class SendEmailNotificationsJob(IMediator mediator) : IJob
    {
        private readonly IMediator _mediator = mediator;

        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new SendPendingEmailNotificationsCommand());
        }
    }
}
