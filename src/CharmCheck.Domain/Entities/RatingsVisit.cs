namespace CharmCheck.Domain.Entities;

public class RatingsVisit
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime LastVisit { get; set; }
}
