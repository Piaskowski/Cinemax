
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class CreateScreeningRequest
    {
        public Guid MovieId { get; set; }
        public Guid CinemaHallId { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public DateTime StartTime { get; set; }
    }
}
