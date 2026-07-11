
using Cinemax.Shared.Contracts.Genres;
using MediatR;

namespace Cinemax.Application.Elements.Genres.Queries
{
    public record GetGenresQuery : IRequest<GetGenresResponse>;
}
