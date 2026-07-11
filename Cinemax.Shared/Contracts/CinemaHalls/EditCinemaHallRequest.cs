using Cinemax.Shared.Enums;

namespace Cinemax.Shared.Contracts.CinemaHalls
{
    public class EditCinemaHallRequest
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public CinemaHallType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
