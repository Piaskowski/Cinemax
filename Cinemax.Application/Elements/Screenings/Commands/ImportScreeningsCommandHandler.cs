using Cinemax.Application.Common;
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Movies.Repositories;
using Cinemax.Application.Elements.Screenings.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using Cinemax.Shared.Contracts.Scrennings;
using Cinemax.Shared.Resources.Common;
using Cinemax.Shared.Resources.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Elements.Screenings.Commands
{
    public class ImportScreeningsCommandHandler(IScreeningRepository screenings,
        IMovieRepository movies,
        ICinemaHallRepository halls,
        IMediator mediator) : IRequestHandler<ImportScreeningsCommand, IEnumerable<string>>
    {
        private readonly IScreeningRepository _screenings = screenings;
        private readonly IMovieRepository _movies = movies;
        private readonly ICinemaHallRepository _halls = halls;

        private readonly IMediator _mediator = mediator;
        public async Task<IEnumerable<string>> Handle(ImportScreeningsCommand command, CancellationToken ct)
        {
            // == read file ==
            var result = await ExcelConverter.ReadSheet<ScreeningExcelDto>(command.Stream);

            // == return errors if any ==
            if (result.Errors.Count > 0)
                return result.Errors;

            var errors = new List<string>();
            var screeningsToCreate = new List<Screening>();

            var rowNumber = 1;

            foreach (var item in result.Items) {

                rowNumber++;

                if (item.StartTime <= DateTime.UtcNow)
                    throw new ValidationException([ValidationMessages.FutureDate]);

                var movie = await _movies.Query().FirstOrDefaultAsync(m => m.Title.ToUpper().Equals(item.MovieTitle.ToUpper()), ct);

                if (movie == null)
                {
                    errors.Add($"{CommonMessages.Txt_Row} {rowNumber}: {ValidationMessages.Movie_NotExists}");
                    continue;
                }


                var endTime = item.StartTime.AddMinutes(movie.DurationMinutes);

                var cinemahall = await _halls.Query().FirstOrDefaultAsync(c => c.Number == item.Number);

                if (cinemahall == null)
                {
                    errors.Add($"{CommonMessages.Txt_Row} {rowNumber}: {ValidationMessages.Cinemahall_NotExists}");
                    continue;
                }

                var screeningsThatCollide = await GetCollidingScreenings(cinemahall, movie, item.StartTime, screeningsToCreate, ct);
                if (screeningsThatCollide != null && screeningsThatCollide.Count > 0)
                {
                    foreach (var screening in screeningsThatCollide)
                    {
                        var start = screening.StartTime.TimeOfDay;
                        var end = screening.StartTime.AddMinutes(screening.Movie.DurationMinutes).TimeOfDay;
                        errors.Add(string.Format($"{CommonMessages.Txt_Row} {rowNumber}:" + ValidationMessages.Screening_DateCollision, start, end));
                    }
                }

                var newScreening = new Screening
                {
                    MovieId = movie.Id,
                    Movie = movie,
                    CinemaHallId = cinemahall.Id,
                    StartTime = item.StartTime,
                };
                
                screeningsToCreate.Add(newScreening);
            }

            // == return errors if any ==
            if (errors.Count > 0)
                return errors;

            foreach (var screening in screeningsToCreate)
                await _screenings.CreateAsync(screening, ct);

            return [];
        }

        private async Task<List<Screening>?> GetCollidingScreenings(CinemaHall cinemahall,
            Movie movie, DateTime startTime, List<Screening> screenings, CancellationToken ct)
        {
            var endTime = startTime.AddMinutes(movie.DurationMinutes);

            var screeningsThatCollide = await _screenings.Query()
                .Include(s => s.Movie)
                .Where(s =>
                    s.CinemaHallId.Equals(cinemahall.Id) &&
                    startTime < s.StartTime.AddMinutes(s.Movie.DurationMinutes) &&
                    endTime > s.StartTime
                ).ToListAsync(ct);

            var pendingScreenings = screenings
                .Where(s =>
                    s.CinemaHallId.Equals(cinemahall.Id) &&
                    startTime < s.StartTime.AddMinutes(s.Movie.DurationMinutes) &&
                    endTime > s.StartTime
                ).ToList();

            screeningsThatCollide.AddRange(pendingScreenings);

            return screeningsThatCollide;
        }
    }
}
