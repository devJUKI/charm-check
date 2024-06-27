using CharmCheck.Application.DTOs;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Queries.GetPhotosAnalytics;

public record GetPhotosAnalyticsQuery(string UserId, string ImageBaseUrl) : IRequest<Result<List<PhotoAnalyticsEntry>>>;