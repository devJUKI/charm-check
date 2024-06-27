using System.Security.Claims;

namespace CharmCheck.Application.Authentication;

public static class ClaimsExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        string? userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new ArgumentNullException(userId);
        }

        return userId;
    }
}
