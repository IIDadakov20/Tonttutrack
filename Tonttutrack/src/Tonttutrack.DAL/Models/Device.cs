using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tonttutrack.DAL.Models;

public class Device
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(17)]
    public string MacAddress { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new HashSet<User>();
}