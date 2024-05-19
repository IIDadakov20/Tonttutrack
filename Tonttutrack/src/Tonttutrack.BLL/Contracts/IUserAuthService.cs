namespace Tonttutrack.BLL.Contracts;

public interface IUserAuthService
{
    Task<bool> CheckUserExistsByEmail(string email);

    Task<bool> CheckUserExistsByUsername(string username);

    Task<bool> VerifyUserPassword(string email, string password);
}