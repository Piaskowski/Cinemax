using Cinemax.Shared.Contracts.Genres;
using MediatR;

namespace Cinemax.Application.Elements.Genres.Commands
{
    public record EditGenreCommand(EditGenreRequest Request) : IRequest;
}
