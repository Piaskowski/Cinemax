using Cinemax.Shared.Enums;

namespace Cinemax.Domain.Entities
{
    public class CinemaHall
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public CinemaHallType Type { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Seat> Seats { get; set; } = [];
    }
}
