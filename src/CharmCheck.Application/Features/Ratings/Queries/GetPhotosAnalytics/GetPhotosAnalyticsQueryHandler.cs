using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Queries.GetPhotosAnalytics;

internal class GetPhotosAnalyticsQueryHandler : IRequestHandler<GetPhotosAnalyticsQuery, Result<List<PhotoAnalyticsEntry>>>
{
    private readonly IGenericRepository<Rating> _ratingRepository;

    public GetPhotosAnalyticsQueryHandler(IGenericRepository<Rating> ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<Result<List<PhotoAnalyticsEntry>>> Handle(GetPhotosAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var ratingsWithReviewers = await _ratingRepository
            .GetSet()
            .Include(pr => pr.Reviewer)
            .Include(pr => pr.ProfileImage)
            .Where(pr => pr.ProfileImage.UserId == request.UserId)
            .ToListAsync();

        var groupedByPhoto = ratingsWithReviewers.GroupBy(pr => pr.ProfileImageId);

        var result = new Dictionary<string, (string Filename, Dictionary<int, Dictionary<int, int>> AgeGroups)>();

        foreach (var photoGroup in groupedByPhoto)
        {
            var photoId = photoGroup.Key;
            var filename = photoGroup.First().ProfileImage.FileName;

            var ageGroups = photoGroup.GroupBy(pr => pr.Reviewer.Age);
            var ageGroupsDict = new Dictionary<int, Dictionary<int, int>>();

            foreach (var ageGroup in ageGroups)
            {
                var age = ageGroup.Key;

                var ratingGroups = ageGroup.GroupBy(pr => pr.PhotoRating);
                var ratingsDict = new Dictionary<int, int>();

                foreach (var ratingGroup in ratingGroups)
                {
                    var rating = ratingGroup.Key;
                    var count = ratingGroup.Count();

                    ratingsDict[rating] = count;
                }

                ageGroupsDict[age] = ratingsDict;
            }

            result[photoId] = (filename, ageGroupsDict);
        }

        var output = result
            .Select(r => new PhotoAnalyticsEntry(
                r.Key,
                $"{request.ImageBaseUrl}{r.Value.Filename}",
                r.Value.AgeGroups))
            .ToList();

        return output;
    }
}
