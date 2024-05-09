using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Tonttutrack.DAL.Data.Models;

namespace Tonttutrack.DAL.Data;

public class TonttutrackDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public TonttutrackDbContext(DbContextOptions<TonttutrackDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }

    public override DbSet<User> Users { get; set; }

    public DbSet<UserDevice> UserDevices { get; set; }

    public DbSet<Route> Routes { get; set; }

    public DbSet<RoutePoint> RoutePoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDevice>()
            .HasKey(ud => new { ud.UserId, ud.DeviceId });

        modelBuilder.Entity<UserDevice>()
            .HasOne(ud => ud.User)
            .WithMany(u => u.UserDevices)
            .HasForeignKey(ud => ud.UserId);

        modelBuilder.Entity<UserDevice>()
            .HasOne(ud => ud.Device)
            .WithMany(u => u.UserDevices)
            .HasForeignKey(ud => ud.DeviceId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Routes)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Route>()
            .HasMany(r => r.RoutePoints)
            .WithOne(rp => rp.Route)
            .HasForeignKey(rp => rp.RouteId);

        base.OnModelCreating(modelBuilder);
    }
}