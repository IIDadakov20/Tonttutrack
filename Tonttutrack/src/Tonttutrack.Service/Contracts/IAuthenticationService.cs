using Tonttutrack.Domain.DTOs.Authentication;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IAuthenticationService
{
    Task<ErrorDTO> RegisterUserAsync(RegisterDTO registerInfo);

    Task<ErrorDTO> LoginUserAsync(LoginDTO loginInfo);

    Task UserSignOutAsync();
}