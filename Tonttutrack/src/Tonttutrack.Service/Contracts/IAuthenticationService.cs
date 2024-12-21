namespace Tonttutrack.Service.Contracts;

public interface IAuthenticationService
{
    Task<bool> CheckUserExistsByEmailAsync(string email);

    Task<bool> CheckUserExistsByUsernameAsync(string username);

    Task<bool> VerifyUserCredentialsAsync(string email, string password);

    Task UserSignInAsync(string email);

    Task UserSignOutAsync();
}