namespace CharmCheck.Domain.Entities;

public class Rating
{
    public int Id { get; set; }
    public User Reviewer { get; set; } = null!; // User who gave the rating
    public string ReviewerId { get; set; } = null!;
    public ProfilePhoto ProfileImage { get; set; } = null!;
    public string ProfileImageId { get; set; } = null!;
    public int PhotoRating { get; set; }
    public DateTime RatingDate { get; set; }
}
