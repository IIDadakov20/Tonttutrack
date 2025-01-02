namespace Tonttutrack.Domain.DTOs.Response;

public class ErrorDTO
{
    public bool Succeeded { get; set; }
    public Dictionary<string, string> ErrorMessage { get; set; } = new();
}