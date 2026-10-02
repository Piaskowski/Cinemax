using Cinemax.Application.Abstractions.Interfaces;
using Cinemax.Shared.Contracts.Orders;
using MediatR;

namespace Cinemax.Application.Elements.Orders.Queries
{
    public class GetUserOrderFormDataQueryHandler(
        ICurrentUserService userService) : IRequestHandler<GetUserOrderFormDataQuery, GetUserOrderFormDataResponse>
    {
        private readonly ICurrentUserService _userService = userService;
        public Task<GetUserOrderFormDataResponse> Handle(GetUserOrderFormDataQuery query, CancellationToken ct)
        {
            var result = new GetUserOrderFormDataResponse
            {
                Email = _userService.Email!
            };

            return Task.FromResult(result);
        }
    }
}
