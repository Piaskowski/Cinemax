using Cinemax.Shared.Contracts.CinemaHalls;
using Cinemax.Shared.Contracts.Movies;
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class ScreeningDto
    {
        public Guid Id { get; set; }
        public required MovieSelectItemDto Movie { get; set; }
        public required CinemaHallSelectItemDto CinemaHall { get; set; }
        public DateTime StartTime { get; set; }
        public ScreeningStatus Status { get; set; }
    }
}
