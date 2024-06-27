using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CharmCheck.Application.Features.Profile.Commands.UploadPhoto;

internal class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, Result<PhotoEntry>>
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<ProfilePhoto> _profileImageRepository;

    public UploadPhotoCommandHandler(IGenericRepository<ProfilePhoto> profileImageRepository, IGenericRepository<User> userRepository)
    {
        _profileImageRepository = profileImageRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<PhotoEntry>> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        string imageFolderPath = Path.Combine(request.ContentRootPath, "Images");

        try
        {
            if (request.FileStream == null || request.FileStream.Length == 0)
            {
                return Result.Failure<PhotoEntry>(DomainErrors.Profile.EmptyPhotoFile);
            }
            
            if (!Directory.Exists(imageFolderPath))
            {
                Directory.CreateDirectory(imageFolderPath);
            }

            var fileExtension = Path.GetExtension(request.FileName);
            var id = Guid.NewGuid().ToString();
            var fileName = id + fileExtension;
            var filePath = Path.Combine(imageFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.FileStream.CopyToAsync(stream, cancellationToken);
            }

            var profileImage = new ProfilePhoto
            {
                Id = id,
                Extension = fileExtension,
                UserId = request.UserId
            };

            await _profileImageRepository.AddAsync(profileImage);
            await _profileImageRepository.SaveChangesAsync();

            var imageUrl = $"{request.ImageBaseUrl}/{fileName}";
            var photoEntry = new PhotoEntry(id, imageUrl);

            return Result.Success(photoEntry);
        }
        catch (Exception ex)
        {
            var error = new Error("UploadImageCommandHandler.Handle", $"Internal server error: {ex}");
            return Result.Failure<PhotoEntry>(error);
        }
    }
}
