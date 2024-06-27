using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Queries.GetRatePhoto;

internal class GetRatePhotoQueryHandler : IRequestHandler<GetRatePhotoQuery, Result<PhotoEntry>>
{
    private readonly IGenericRepository<ProfilePhoto> _photoRepository;

    public GetRatePhotoQueryHandler(IGenericRepository<ProfilePhoto> photoRepository)
    {
        _photoRepository = photoRepository;
    }

    public async Task<Result<PhotoEntry>> Handle(GetRatePhotoQuery request, CancellationToken cancellationToken)
    {
        // This is just a placeholder that returns random photo before real algorithm is implemented

        var photo = _photoRepository
            .GetSet()
            .OrderBy(e => Guid.NewGuid())
            .FirstOrDefault();

        if (photo == null)
        {
            return Result.Failure<PhotoEntry>(new Error("No Photos", "No photos found"));
        }

        var photoEntry = new PhotoEntry(photo.Id, $"{request.ImageBaseUrl}/{photo.FileName}");
        return photoEntry;
    }
}
