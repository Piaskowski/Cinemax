using Cinemax.Shared.Contracts.Genres;
using MediatR;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public record CreateGenreCommand(CreateGenreRequest Request) : IRequest;
}
