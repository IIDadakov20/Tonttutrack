using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.Domain.DTOs.Response;

public class RouteResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Date { get; set; }

    public double Distance { get; set; }

    public TimeSpan Duration { get; set; }

    public ICollection<RoutePoint> RoutePoints { get; set; } = null!;
}
