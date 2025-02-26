using Tonttutrack.Domain.DTOs.Request;
using Tonttutrack.Domain.DTOs.Response;

namespace Tonttutrack.Service.Contracts;

public interface IUserService
{
    Task<ErrorDTO> UpdateUserAsync(UserRequestDTO userInfo);

    Task<ErrorDTO> ChangeUserPasswordAsync(UserPasswordRequestDTO passwordInfo);

    Task<ErrorDTO> DeleteUserAsync(string password);
}