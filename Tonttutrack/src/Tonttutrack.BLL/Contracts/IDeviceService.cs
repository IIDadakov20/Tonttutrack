namespace Tonttutrack.BLL.Contracts;

public interface IDeviceService
{
    Task<bool> VerifyDeviceCredentialsAsync(string code, string password);

    Task<string?> FetchConnectedDeviceName(string code);
}