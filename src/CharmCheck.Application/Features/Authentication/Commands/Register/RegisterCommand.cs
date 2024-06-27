using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    int Age,
    int Gender) : IRequest<Result<string>>;
