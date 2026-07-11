using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Application.Common;
using Cinemax.Application.Elements.CinemaHalls.Repositories;
using Cinemax.Application.Elements.Seats.Repositories;
using Cinemax.Application.Exceptions;
using Cinemax.Domain.Entities;
using Cinemax.Shared.Contracts.Seats;
using MediatR;

namespace Cinemax.Application.Elements.Seats.Commands
{
    public class ImportSeatsCommandHandler(ISeatRepository repository,
        ICinemaHallRepository hallRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ImportSeatsCommand, IEnumerable<string>>
    {
        private readonly ISeatRepository _repository = repository;
        private readonly ICinemaHallRepository _hallRepository = hallRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<string>> Handle(ImportSeatsCommand command, CancellationToken ct)
        {
            // == read file ==
            var result = await ExcelConverter.ReadSheet<SeatExcelDto>(command.Stream);
            
            var duplicatedSeatsErrors = result.Items
                .GroupBy(x => new { x.Row, x.Number })
                .Where(g => g.Count() > 1)
                .Select(g => $"Miejsce w rzędzie {g.Key.Row}, numer {g.Key.Number} występuje {g.Count()} razy.")
                .ToList();

            result.Errors.AddRange(duplicatedSeatsErrors);

            // == return errors if any ==
            if (result.Errors.Count > 0)
                return result.Errors;

            // == import seats ==
            var cinemahall = await _hallRepository.GetByIdAsync(command.CinemahallId) ??
                throw new ValidationException(["Dana sala kinowa nie istnieje"]);

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var existingSeats = await _repository.GetSeatsByCinemahall(cinemahall.Id, ct);
                var existingSeatsByKey = existingSeats
                    .ToDictionary(x => new { x.Row, x.Number });

                var importedSeatsByKey = result.Items
                    .ToDictionary(x => new { x.Row, x.Number });

                foreach (var importedSeat in result.Items)
                {
                    var key = new { importedSeat.Row, importedSeat.Number };

                    if (existingSeatsByKey.TryGetValue(key, out var existingSeat))
                    {
                        existingSeat.Type = importedSeat.Type;
                        existingSeat.IsActive = true;
                    }
                    else
                    {
                        var newSeat = new Seat
                        {
                            Row = importedSeat.Row,
                            Number = importedSeat.Number,
                            Type = importedSeat.Type,
                            CinemaHall = cinemahall
                        };

                        await _repository.CreateAsync(newSeat, ct);
                    }
                }

                var seatsToDeactivate = existingSeats
                    .Where(existingSeat =>
                    {
                        var key = new { existingSeat.Row, existingSeat.Number };
                        return !importedSeatsByKey.ContainsKey(key);
                    })
                    .ToList();

                foreach (var seat in seatsToDeactivate)
                {
                    seat.IsActive = false;
                }
                await _unitOfWork.CommitTransactionAsync(ct);

                return [];
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
