using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tonttutrack.DAL.Data.Models;

public class RoutePoint
{
    public RoutePoint()
    {
        Id = Guid.NewGuid();
        RecordedAt = DateTime.Now;
    }

    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime RecordedAt { get; set; }
    [Required]
    [Precision(9, 6)]
    public decimal Latitude { get; set; }
    [Required]
    [Precision(9, 6)]
    public decimal Longitude { get; set; }
    [Required]
    [Precision(5, 2)]
    public decimal CurrentSpeed { get; set; }

    public Guid RouteId { get; set; }
    public Route Route { get; set; } = null!;
}
