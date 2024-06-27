using CharmCheck.Application.Features.Profile.Commands.UploadPhoto;
using CharmCheck.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using CharmCheck.Application.Authentication;
using CharmCheck.Application.Features.Profile.Queries.GetPhotos;
using CharmCheck.Application.DTOs;
using CharmCheck.Application.Features.Profile.Commands.DeletePhoto;

namespace CharmCheck.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISender _sender;

        public ProfileController(IWebHostEnvironment environment, ISender sender)
        {
            _environment = environment;
            _sender = sender;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Photos")]
        public async Task<IActionResult> GetPhotos()
        {
            var userId = User.GetUserId();
            var imageBaseUrl = $"{Request.Scheme}://{Request.Host}/Images/";
            var query = new GetPhotosQuery(userId, imageBaseUrl);
            var result = await _sender.Send(query);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            List<PhotoEntry> photoEntries = result.Value;
            return Ok(new { photoEntries });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Photos")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            var imageBaseUrl = $"{Request.Scheme}://{Request.Host}/Images/";
            var userId = User.GetUserId();
            var command = new UploadPhotoCommand(
                _environment.ContentRootPath,
                file.FileName,
                file.OpenReadStream(),
                userId,
                imageBaseUrl);
            var result = await _sender.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            PhotoEntry photoEntry = result.Value;
            return Ok(new { photoEntry });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("Photos/{id}")]
        public async Task<IActionResult> DeletePhoto(string id)
        {
            var userId = User.GetUserId();
            var command = new DeletePhotoCommand(
                _environment.ContentRootPath,
                id,
                userId);
            var result = await _sender.Send(command);

            return result.Match<IActionResult>(onSuccess: Ok, onFailure: BadRequest);
        }
    }
}
