
namespace Cinemax.Application.Abstractions.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? Id { get; }
        string? Email { get; }
    }
}
