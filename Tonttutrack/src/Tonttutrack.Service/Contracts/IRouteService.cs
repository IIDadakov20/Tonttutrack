using System.Text.Json;
using System.Threading.Tasks;
using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IRouteService
{
    Task<List<RouteResponseDTO>> GetUserRoutesAsync(int pageNumber, string searchTerm);

    Task<int> GetUserRoutesNumberAsync(string searchTerm);

    Task<ErrorDTO> CreateRouteAsync();

    Task<ErrorDTO> UpdateRouteAsync(RouteRequestDTO route);

    Task<ErrorDTO> DeleteRouteAsync(Guid routeId);

    Task<bool> DeleteUserRoutesAsync(Guid userId);

    Task<ErrorDTO> SaveRoutePointAsync(JsonElement data);
}