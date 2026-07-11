using Cinemax.Shared.Enums;

namespace Cinemax.Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid CinemaHallId { get; set; }
        public required CinemaHall CinemaHall { get; set; }
        public int Number { get; set; }
        public int Row { get; set; }
        public SeatType Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
