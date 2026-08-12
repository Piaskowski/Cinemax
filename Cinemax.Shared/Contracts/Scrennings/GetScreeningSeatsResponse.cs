using Cinemax.Shared.Contracts.Seats;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class GetScreeningSeatsResponse
    {
        public int CinemaHallNumber { get; set; } = default!;
        public string MovieTitle { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public string PosterUrl { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public IEnumerable<ScreeningSeatsRowDto> SeatsRows { get; set; } = [];
    }
}
