namespace Tonttutrack.BLL.Contracts;

public interface IAuthenticationService
{
    Task<bool> CheckUserExistsByEmailAsync(string email);

    Task<bool> CheckUserExistsByUsernameAsync(string username);

    Task<bool> VerifyUserPasswordAsync(string email, string password);

    Task UserSignInAsync(string email);

    Task UserSignOutAsync();
}