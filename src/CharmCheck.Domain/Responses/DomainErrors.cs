namespace CharmCheck.Domain.Responses;

public static class DomainErrors
{
    public static class Auth
    {
        public static readonly Error FailedRegister = new("Authentication.FailedRegistration", "Registration was not successful");
        public static readonly Error FailedLogin = new("Authentication.FailedLogin", "Invalid credentials");
        public static readonly Error FailedToRetrieveEmail = new("Authentication.FailedToRetrieveEmail", "Failed to retrieve email");
        public static readonly Error Unauthorized = new("Authorization.Unauthorized", "User is not owner of this resource");
    }

    public static class Profile
    {
        public static readonly Error EmptyPhotoFile = new("UploadPhoto.EmptyPhotoFile", "Photo file was empty");
        public static readonly Error PhotoNotFound = new("DeletePhoto.PhotoNotFound", "Photo was not found");
    }

    public static class User
    {
        public static readonly Error FirstnameEmpty = new("UserCreate.Firstname", "First name cannot be empty");
        public static readonly Error AgeIsLessThan18 = new("UserCreate.Age", "Age must be at least 18");
        public static readonly Error UnknownGender = new("UserCreate.Gender", "Unknown gender");
    }
}
