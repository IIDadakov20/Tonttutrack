using Tonttutrack.DataAccess.Data.Models;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface ICurrentUserService
{
    UserResponseDTO CurrentUser { get; }
}
