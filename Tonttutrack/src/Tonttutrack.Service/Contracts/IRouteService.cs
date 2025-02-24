using System.Text.Json;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IRouteService
{
    Task<ErrorDTO> CreateRouteAsync();

    Task<ErrorDTO> DeleteRouteAsync(Guid id);

    Task<ErrorDTO> SaveRoutePointAsync(JsonElement data);
}