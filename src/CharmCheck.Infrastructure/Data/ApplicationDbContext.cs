using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CharmCheck.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharmCheck.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<ProfilePhoto> Photos { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.ProfileImage)
            .WithMany()
            .HasForeignKey(r => r.ProfileImageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProfilePhoto>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
