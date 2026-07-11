
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class EditScreeningCommandHandler(IScreeningRepository screenings,
        IMovieRepository movies) : IRequestHandler<EditScreeningCommand>
    {
        private readonly IScreeningRepository _screenings = screenings;
        private readonly IMovieRepository _movies = movies;
        public async Task Handle(EditScreeningCommand command, CancellationToken ct)
        {
            var request = command.Request;
            if (request.StartTime <= DateTime.UtcNow)
                throw new ValidationException(["Data seansu nie może być przeszła."]);

            var movie = await _movies.GetByIdAsync(request.MovieId, ct) ?? 
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

            var editedScreening = await _screenings.GetByIdAsync(request.Id, ct) ??
                                throw new ValidationException(["Wskazany seans nie istnieje."]);

            editedScreening.CinemaHallId = request.CinemaHallId;
            editedScreening.MovieId = request.MovieId;
            editedScreening.StartTime = request.StartTime;
            editedScreening.Status = request.Status;

            await _screenings.UpdateAsync(editedScreening, ct);
        }
    }
}
