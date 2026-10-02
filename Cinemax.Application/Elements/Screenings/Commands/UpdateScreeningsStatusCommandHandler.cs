using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class UpdateScreeningsStatusCommandHandler(IScreeningRepository screenings) : IRequestHandler<UpdateScreeningsStatusCommand>
    {
        private readonly IScreeningRepository _screenings = screenings;
        public async Task Handle(UpdateScreeningsStatusCommand command, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            await _screenings.Query()
                .Where(s =>
                    s.Status == ScreeningStatus.Scheduled &&
                    s.StartTime.AddMinutes(s.Movie.DurationMinutes) <= now)
                .ExecuteUpdateAsync(
                    x => x.SetProperty(s => s.Status, ScreeningStatus.Finished),ct);
        }
    }
}

