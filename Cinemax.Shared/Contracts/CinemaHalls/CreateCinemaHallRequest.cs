using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.CinemaHalls
{
    public class CreateCinemaHallRequest
    {
        public int Number { get; set; }
        public CinemaHallType Type { get; set; }
    }
}
