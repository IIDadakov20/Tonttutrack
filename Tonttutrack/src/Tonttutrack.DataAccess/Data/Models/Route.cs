using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tonttutrack.DataAccess.Data.Models;

public class Route
{
    public Route()
    {
        Id = Guid.NewGuid();
        Date = DateOnly.FromDateTime(DateTime.Now);

        RoutePoints = new HashSet<RoutePoint>();
    }

    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public double Distance { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public ICollection<RoutePoint> RoutePoints { get; set; } = null!;
}