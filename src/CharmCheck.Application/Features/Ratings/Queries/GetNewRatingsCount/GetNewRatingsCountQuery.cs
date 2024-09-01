using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Queries.GetNewRatingsCount;

public record GetNewRatingsCountQuery(string UserId) : IRequest<Result<int>>;