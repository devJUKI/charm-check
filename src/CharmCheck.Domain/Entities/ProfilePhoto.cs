namespace CharmCheck.Domain.Entities;

public class ProfilePhoto
{
    public string Id { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public User User { get; set; } = null!;
    public string UserId { get; set; } = null!;

    public string FileName => Id.ToString() + Extension;
}