namespace Tonttutrack.Domain.DTOs.Response;

public class DeviceResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;
}