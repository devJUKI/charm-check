using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Profile.Commands.UploadPhoto;

public record UploadPhotoCommand(
    string ContentRootPath,
    string FileName,
    Stream FileStream,
    string UserId,
    string ImageBaseUrl) : IRequest<Result<PhotoEntry>>;