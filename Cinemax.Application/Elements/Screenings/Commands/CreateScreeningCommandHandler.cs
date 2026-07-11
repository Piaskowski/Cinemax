using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
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
                throw new ValidationException(["Data seansu nie może być przeszła."]);

            var movie = await _movies.GetByIdAsync(request.MovieId, ct);

            if (movie == null)
                throw new ValidationException(["Wskazany film nie istnieje."]);

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
                    var end = screening.StartTime.AddDays(screening.Movie.DurationMinutes).TimeOfDay;
                    errors.Add("Kolizja z " + start + " - " + end);
                }
                throw new ValidationException(errors);
            }

            var newScreening = new Screening
            {
                MovieId = request.MovieId,
                CinemaHallId = request.CinemaHallId,
                StartTime = request.StartTime,
            };

            await _screenings.CreateAsync(newScreening, ct);
        }
    }
}
