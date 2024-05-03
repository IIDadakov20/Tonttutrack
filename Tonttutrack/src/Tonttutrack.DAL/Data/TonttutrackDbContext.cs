using Microsoft.EntityFrameworkCore;
using Tonttutrack.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Tonttutrack.DAL.Data;

public class TonttutrackDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public TonttutrackDbContext(DbContextOptions<TonttutrackDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }

    public override DbSet<User> Users { get; set; }

    public DbSet<UserDevice> UserDevices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure UserDevice as a junction table
        modelBuilder.Entity<UserDevice>()
           .HasKey(ud => new { ud.UserId, ud.DeviceId }); // Composite key configuration

        modelBuilder.Entity<User>()
        .HasMany(u => u.Devices)
        .WithMany(d => d.Users)
        .UsingEntity<UserDevice>();
    }
}