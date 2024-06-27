using CharmCheck.Domain.Enums;
using CharmCheck.Domain.Responses;
using Microsoft.AspNetCore.Identity;

namespace CharmCheck.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; private set; } = null!;
    public int Age { get; private set; }
    public Gender Gender { get; private set; }

    private User() { }

    public static Result<User> Create(string email, string firstName, int age, int gender)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<User>(DomainErrors.User.FirstnameEmpty);

        if (age < 18)
            return Result.Failure<User>(DomainErrors.User.AgeIsLessThan18);

        if (!Enum.IsDefined(typeof(Gender), gender))
            return Result.Failure<User>(DomainErrors.User.UnknownGender);

        Gender genderEnum = (Gender)gender;

        var user = new User()
        {
            FirstName = firstName,
            Age = age,
            Email = email,
            UserName = email,
            Gender = genderEnum
        };

        return user;
    }
}