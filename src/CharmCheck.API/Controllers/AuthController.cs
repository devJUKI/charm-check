using CharmCheck.Application.Features.Authentication.Commands.Register;
using CharmCheck.Application.Features.Authentication.Queries.Login;
using Microsoft.AspNetCore.Mvc;
using CharmCheck.API.Models;
using MediatR;

namespace CharmCheck.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var command = new RegisterCommand(
                model.Email,
                model.Password,
                model.FirstName,
                model.Age,
                model.Gender);

            var result = await _sender.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { result.Value });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var command = new LoginQuery(
                model.Email,
                model.Password);

            var result = await _sender.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { result.Value });
        }
    }
}
