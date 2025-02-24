using System.Text.Json;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IRouteService
{
    Task<ErrorDTO> CreateRouteAsync();

    Task<ErrorDTO> SaveRoutePointAsync(JsonElement data);
}