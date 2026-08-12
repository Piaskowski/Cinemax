
namespace Cinemax.Shared.Contracts.Seats
{
    public class ScreeningSeatsRowDto
    {
        public int Row { get; set; }
        public IEnumerable<ScreeningSeatDto> Seats { get; set; } = [];
    }
}
