using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.DataAccess.Data.Models;

public class Device
{
    public Device()
    {
        Id = Guid.NewGuid();

        UserDevices = new HashSet<UserDevice>();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [StringLength(17)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    public ICollection<UserDevice> UserDevices { get; set; }
}