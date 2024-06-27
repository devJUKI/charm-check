using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Queries.GetRatePhoto;

public record GetRatePhotoQuery(string ImageBaseUrl) : IRequest<Result<PhotoEntry>>;
