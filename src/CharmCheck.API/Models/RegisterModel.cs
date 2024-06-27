namespace CharmCheck.API.Models;

public class RegisterModel
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public int Age { get; set; }
    public int Gender { get; set; }
}
