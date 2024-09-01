using CharmCheck.Application.Authentication;
using CharmCheck.Application.DTOs;
using CharmCheck.Application.Features.Ratings.Commands.RatePhoto;
using CharmCheck.Application.Features.Ratings.Queries.GetNewRatingsCount;
using CharmCheck.Application.Features.Ratings.Queries.GetPhotosAnalytics;
using CharmCheck.Application.Features.Ratings.Queries.GetRatePhoto;
using CharmCheck.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharmCheck.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ISender _sender;

        public RatingController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Rate")]
        public async Task<IActionResult> RatePhoto(string photoId, int photoRating)
        {
            var userId = User.GetUserId();
            var command = new RatePhotoCommand(userId, photoId, photoRating);
            var result = await _sender.Send(command);

            return result.Match<IActionResult>(onSuccess: Ok, onFailure: BadRequest);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Photo")]
        public async Task<IActionResult> GetRatePhoto()
        {
            var imageBaseUrl = $"{Request.Scheme}://{Request.Host}/Images";
            var query = new GetRatePhotoQuery(imageBaseUrl);
            var result = await _sender.Send(query);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            PhotoEntry photoEntry = result.Value;
            return Ok(new { photoEntry });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Analytics")]
        public async Task<IActionResult> GetPhotosAnalytics()
        {
            var imageBaseUrl = $"{Request.Scheme}://{Request.Host}/Images";
            var userId = User.GetUserId();
            var query = new GetPhotosAnalyticsQuery(userId, imageBaseUrl);
            var result = await _sender.Send(query);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var analytics = result.Value;
            return Ok(new { analytics });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("NewRatingsCount")]
        public async Task<IActionResult> GetNewRatingsCount()
        {
            var userId = User.GetUserId();
            var query = new GetNewRatingsCountQuery(userId);
            var result = await _sender.Send(query);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var count = result.Value;
            return Ok(new { count });
        }
    }
}
