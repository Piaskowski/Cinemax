using Cinemax.Application.Elements.Screenings.Commands;
using MediatR;
using Quartz;

namespace Cinemax.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateScreeningsStatusJob(IMediator mediator) : IJob
    {
        private readonly IMediator _mediator = mediator;

        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new UpdateScreeningsStatusCommand());
        }
    }
}
