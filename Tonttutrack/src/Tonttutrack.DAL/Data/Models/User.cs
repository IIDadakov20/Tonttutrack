using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tonttutrack.DAL.Data.Models;

public class User : IdentityUser<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
        IsActive = true;

        UserDevices = new HashSet<UserDevice>();
        Routes = new HashSet<Route>();
    }

    [Required]
    public bool IsActive { get; set; }

    public ICollection<UserDevice> UserDevices { get; set; }
    public ICollection<Route> Routes { get; set; }
}