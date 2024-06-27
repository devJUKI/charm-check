using CharmCheck.Application.Interfaces;
using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Responses;
using Microsoft.AspNetCore.Identity;
using MediatR;
using CharmCheck.Domain.Interfaces;

namespace CharmCheck.Application.Features.Authentication.Queries.Login;

internal class LoginQueryHandler : IRequestHandler<LoginQuery, Result<string>>
{
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGenericRepository<User> _userRepository;

    public LoginQueryHandler(SignInManager<User> signInManager, IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<User> userRepository)
    {
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (result.Succeeded)
        {
            var user = _userRepository
                .GetSet()
                .Single(u => u.Email!.ToLower() == request.Email.ToLower());

            string token = _jwtTokenGenerator.GenerateToken(user.Id, []);
            return Result.Success(token);
        }
        
        return Result.Failure<string>(DomainErrors.Auth.FailedLogin);
    }
}
