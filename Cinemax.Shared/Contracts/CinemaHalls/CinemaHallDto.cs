
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.CinemaHalls
{
    public class CinemaHallDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public CinemaHallType Type { get; set; }
        public int SeatCount { get; set; }
        public bool IsActive { get; set; }
    }
}
