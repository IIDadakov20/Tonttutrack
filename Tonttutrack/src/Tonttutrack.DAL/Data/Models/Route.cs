using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.DAL.Data.Models;

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
    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;
    [Required]
    public DateOnly Date { get; set; }
    [Required]
    public double Distance { get; set; }
    [Required]
    public TimeSpan Duration { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<RoutePoint> RoutePoints { get; set; }
}
