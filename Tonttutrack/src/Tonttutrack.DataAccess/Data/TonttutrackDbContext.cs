using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.DataAccess.Data;

public class TonttutrackDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public TonttutrackDbContext(DbContextOptions<TonttutrackDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }

    public DbSet<UserDevice> UserDevices { get; set; }

    public DbSet<Route> Routes { get; set; }

    public DbSet<RoutePoint> RoutePoints { get; set; }
}