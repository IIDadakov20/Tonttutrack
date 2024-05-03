using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tonttutrack.DAL.Models;

public class User : IdentityUser<Guid>
{
    [Required]
    public bool IsActive { get; set; } = true;

    public ICollection<Device> Devices { get; set; } = new HashSet<Device>();
}