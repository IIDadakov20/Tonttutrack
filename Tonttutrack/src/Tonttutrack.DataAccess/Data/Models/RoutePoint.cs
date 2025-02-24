using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tonttutrack.DataAccess.Data.Models;

public class RoutePoint
{
    public RoutePoint()
    {
        Id = Guid.NewGuid();
        RecordedAt = DateTime.Now.ToUniversalTime();
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

    [Required]
    public Guid RouteId { get; set; }

    [ForeignKey(nameof(RouteId))]
    public Route Route { get; set; } = null!;
}