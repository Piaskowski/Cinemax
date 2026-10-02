using Cinemax.Application.Elements.Orders.Commands;
using MediatR;
using Quartz;

namespace Cinemax.Scheduler.Jobs
{
    public class UpdateOrdersStatusJob(IMediator mediator) : IJob
    {
        private readonly IMediator _mediator = mediator;

        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new UpdateOrdersStatusCommand());
        }
    }
}
