
using IMS.Application.Common.Results;

namespace IMS.Application.Abstractions.Auth
{
    // This interface defines a contract for initializing the identity system,
    // which involve creating an initial admin user or setting up default roles.
    public interface IIdentityInitializationService
    {
        Task<Result<bool>> InitializeAsync(
            string email,
            string password,
            string userName,
            CancellationToken ct);
    }
}
