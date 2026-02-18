
namespace IMS.Application.Abstractions.Auth
{
    // This interface defines the contract for an authentication service that validates user credentials and retrieves user roles.
    // It includes two asynchronous methods: ValidateUserAsync for validating user credentials and GetRolesAsync for retrieving the roles associated with a user.
    public interface IAuthService
    {
        Task<AuthUser?> ValidateUserAsync(string email, string password, CancellationToken ct);
        Task<IReadOnlyList<string>> GetRolesAsync(int userId, CancellationToken ct);

        // This method creates a new user with the specified email, password, and roles. It returns a CreateUserResult containing the details of the newly created user.
        Task<CreateUserResult> CreateUserAsync(string email, string password, IReadOnlyList<string> roles, CancellationToken ct);


    }

    // Application needs the stamp value so it can be embedded in the JWT (without Application touching EF).
    // The stamp value is used to invalidate JWTs when the user changes their password or logs out from all devices.
    public sealed record AuthUser(int UserId, string Email, string SecurityStamp);


    public sealed record CreateUserResult(int UserId, string Email, IReadOnlyList<string> Roles);

}
