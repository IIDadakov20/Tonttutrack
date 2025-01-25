using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tonttutrack.DataAccess.Data;
using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.Web;

public static class ServiceConfiguratorExtensions
{
    public static void AddContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<TonttutrackDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<TonttutrackDbContext>();
    }

    public static void AddCookie(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Authentication/Login";
            options.Cookie.MaxAge = TimeSpan.FromDays(10);
            options.ExpireTimeSpan = TimeSpan.FromDays(10);
            options.Cookie.HttpOnly = true;
        });
    }
}