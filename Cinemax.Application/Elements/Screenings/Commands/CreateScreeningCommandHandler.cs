using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class CreateScreeningCommandHandler(IScreeningRepository screeningRepository,
        IMovieRepository movieRepository) : IRequestHandler<CreateScreeningCommand>
    {
        private readonly IScreeningRepository _screenings = screeningRepository;
        private readonly IMovieRepository _movies = movieRepository;
        public async Task Handle(CreateScreeningCommand command, CancellationToken ct)
        {
            var request = command.Request;
            if (request.StartTime <= DateTime.UtcNow)
                throw new ValidationException([ValidationMessages.FutureDate]);

            var movie = await _movies.GetByIdAsync(request.MovieId, ct)
                ?? throw new ValidationException([ValidationMessages.Movie_NotExists]);

            var endTime = request.StartTime.AddMinutes(movie.DurationMinutes);

            var screeningsThatCollide = await _screenings.Query()
                .Include(s => s.Movie)
                .Where(s =>
                    s.CinemaHallId.Equals(request.CinemaHallId) &&
                    request.StartTime < s.StartTime.AddMinutes(s.Movie.DurationMinutes) &&
                    endTime > s.StartTime
                ).ToListAsync(ct);

            if (screeningsThatCollide.Count > 0)
            {
                var errors = new List<string>();
                foreach (var screening in screeningsThatCollide)
                {
                    var start = screening.StartTime.TimeOfDay;
                    var end = screening.StartTime.AddMinutes(screening.Movie.DurationMinutes).TimeOfDay;
                    errors.Add(string.Format(ValidationMessages.Screening_DateCollision, start, end));
                }
                throw new ValidationException(errors);
            }

            var newScreening = new Screening
            {
                MovieId = request.MovieId,
                CinemaHallId = request.CinemaHallId,
                ScreeningType = request.ScreeningType,
                StartTime = request.StartTime,
            };

            await _screenings.CreateAsync(newScreening, ct);
        }
    }
}
