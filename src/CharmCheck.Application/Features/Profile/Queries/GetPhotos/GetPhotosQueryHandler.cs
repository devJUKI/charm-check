using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Profile.Queries.GetPhotos;

internal class GetPhotosQueryHandler : IRequestHandler<GetPhotosQuery, Result<List<PhotoEntry>>>
{
    private readonly IGenericRepository<ProfilePhoto> _photoRepository;

    public GetPhotosQueryHandler(IGenericRepository<ProfilePhoto> photoRepository)
    {
        _photoRepository = photoRepository;
    }

    public async Task<Result<List<PhotoEntry>>> Handle(GetPhotosQuery request, CancellationToken cancellationToken)
    {
        var photos = await _photoRepository
            .FindAsync(p => p.UserId == request.UserId);

        var photoEntries = photos
            .Select(p => new PhotoEntry(p.Id, $"{request.ImageBaseUrl}/{p.FileName}"))
            .ToList();

        return photoEntries;
    }
}
