using CharmCheck.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using CharmCheck.Domain.Responses;
using CharmCheck.Domain.Entities;
using MediatR;
using CharmCheck.Domain.Interfaces;

namespace CharmCheck.Application.Features.Authentication.Commands.Register;

internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGenericRepository<User> _userRepository;

    public RegisterCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<User> userRepository)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userResult = User.Create(request.Email, request.FirstName, request.Age, request.Gender);

        if (userResult.IsFailure)
        {
            return Result.Failure<string>(userResult.Error);
        }

        var result = await _userManager.CreateAsync(userResult.Value, request.Password);

        if (result.Succeeded)
        {
            var user = _userRepository
                .GetSet()
                .Single(u => u.Email!.ToLower() == request.Email.ToLower());

            string token = _jwtTokenGenerator.GenerateToken(user.Id, []);
            return Result.Success(token);
        }
        
        string errorMessage = result
            .Errors
            .First()
            .Description
            .Replace("Username", "Email");

        return Result.Failure<string>(new(DomainErrors.Auth.FailedRegister.Code, errorMessage));
    }
}
