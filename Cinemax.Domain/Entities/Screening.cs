using Cinemax.Shared.Enums;

namespace Cinemax.Domain.Entities
{
    public class Screening
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = default!;
        public Guid CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public ScreeningStatus Status { get; set; } = ScreeningStatus.Scheduled;
    }
}
