
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class EditScreeningRequest
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid CinemaHallId { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public DateTime StartTime { get; set; }
        public ScreeningStatus Status { get; set; }
    }
}
