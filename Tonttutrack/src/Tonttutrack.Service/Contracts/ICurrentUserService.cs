using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.Service.Contracts;

public interface ICurrentUserService
{
    public User? CurrentUser { get; }
}
