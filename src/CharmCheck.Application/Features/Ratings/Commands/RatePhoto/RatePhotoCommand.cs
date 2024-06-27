using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Commands.RatePhoto;

public record RatePhotoCommand(
    string UserId,
    string PhotoId,
    int PhotoRating) : IRequest<Result>;
