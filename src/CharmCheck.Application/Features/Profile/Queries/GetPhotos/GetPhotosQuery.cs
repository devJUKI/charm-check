using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Profile.Queries.GetPhotos;

public record GetPhotosQuery(string UserId, string ImageBaseUrl) : IRequest<Result<List<PhotoEntry>>>;
