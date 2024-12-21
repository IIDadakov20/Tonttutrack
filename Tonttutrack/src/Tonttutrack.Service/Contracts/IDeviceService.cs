namespace Tonttutrack.Service.Contracts;

public interface IDeviceService
{
    Task<bool> VerifyDeviceCredentialsAsync(string code, string password);

    Task<string?> FetchConnectedDeviceName(string code);
}