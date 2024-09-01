using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CharmCheck.Application.Features.Ratings.Queries.GetNewRatingsCount;

internal class GetNewRatingsCountQueryHandler : IRequestHandler<GetNewRatingsCountQuery, Result<int>>
{
    private readonly IGenericRepository<RatingsVisit> _ratingsVisitRepository;
    private readonly IGenericRepository<Rating> _ratingsRepository;

    public GetNewRatingsCountQueryHandler(
        IGenericRepository<RatingsVisit> ratingsVisitRepository,
        IGenericRepository<Rating> ratingsRepository)
    {
        _ratingsVisitRepository = ratingsVisitRepository;
        _ratingsRepository = ratingsRepository;
    }

    public async Task<Result<int>> Handle(GetNewRatingsCountQuery request, CancellationToken cancellationToken)
    {
        var userVisit = await _ratingsVisitRepository
            .GetSet()
            .SingleOrDefaultAsync(v => v.UserId == request.UserId, cancellationToken);

        var newRatingsCount = await _ratingsRepository
            .GetSet()
            .Include(r => r.ProfileImage)
            .CountAsync(r => r.ProfileImage.UserId == request.UserId
                && r.RatingDate > userVisit!.LastVisit, cancellationToken);

        return newRatingsCount;
    }
}
