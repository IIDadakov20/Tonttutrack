using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.DAL.Data.Models;

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
    [Unicode(false)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(25)]
    [Unicode(false)]
    public string Password { get; set; } = null!;
    [Required]
    [StringLength(17)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    public ICollection<UserDevice> UserDevices { get; set; }
}