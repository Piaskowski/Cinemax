
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class ShowtimeDto
    {
        public Guid Id { get; set; }
        public CinemaHallType Type { get; set; }
        public DateTime StartTime { get; set; }
    }
}
