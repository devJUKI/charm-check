namespace CharmCheck.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, List<string> roles);
}
