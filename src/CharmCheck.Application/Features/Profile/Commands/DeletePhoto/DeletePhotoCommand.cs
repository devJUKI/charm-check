using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Profile.Commands.DeletePhoto;

public record DeletePhotoCommand(string ContentRootPath, string Id, string UserId) : IRequest<Result>;
