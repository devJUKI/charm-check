using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<Result<string>>;
