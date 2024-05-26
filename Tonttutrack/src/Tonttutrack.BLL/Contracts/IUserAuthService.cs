namespace Tonttutrack.BLL.Contracts;

public interface IUserAuthService
{
    Task<bool> CheckUserExistsByEmailAsync(string email);

    Task<bool> CheckUserExistsByUsernameAsync(string username);

    Task<bool> VerifyUserPasswordAsync(string email, string password);
}