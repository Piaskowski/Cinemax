
using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.Seats
{
    public class ScreeningSeatDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Row { get; set; }
        public SeatType Type { get; set; }
        public ScreeningSeatStatus Status { get; set; }
    }
}
