
using Cinemax.Shared.Contracts.Movies;

namespace Cinemax.Shared.Contracts.Scrennings
{
    public class GetScreeningsByDateResponse
    {
        public IEnumerable<MovieShowtimesDto> MovieShowtimes { get; set; } = [];
    }
}
