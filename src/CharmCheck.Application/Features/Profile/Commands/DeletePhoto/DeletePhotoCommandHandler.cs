using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Profile.Commands.DeletePhoto;

internal class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Result>
{
    private readonly IGenericRepository<ProfilePhoto> _photoRepository;

    public DeletePhotoCommandHandler(IGenericRepository<ProfilePhoto> photoRepository)
    {
        _photoRepository = photoRepository;
    }

    public async Task<Result> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = _photoRepository
            .GetSet()
            .FirstOrDefault(p => p.Id == request.Id);

        if (photo == null)
        {
            return Result.Failure(DomainErrors.Profile.PhotoNotFound);
        }

        if (photo.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.Auth.Unauthorized);
        }

        _photoRepository.Delete(photo);
        await _photoRepository.SaveChangesAsync();

        string imageFolderPath = Path.Combine(request.ContentRootPath, "Images");
        string photoPath = Path.Combine(imageFolderPath, photo.FileName);

        // Print to logs if doesn't exist
        if (File.Exists(photoPath))
        {
            File.Delete(photoPath);
        }

        return Result.Success();
    }
}
