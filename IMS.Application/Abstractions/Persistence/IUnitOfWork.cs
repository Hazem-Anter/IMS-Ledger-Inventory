
namespace IMS.Application.Abstractions.Persistence
{
    // IUnitOfWork interface defines a contract for a unit of work pattern, which is used to group multiple operations into a single transaction.
    // The SaveChangesAsync method commits all changes made in the context to the database asynchronously.
    // This pattern helps ensure data integrity and consistency by allowing multiple operations to be treated as a single unit.

    // The CancellationToken parameter allows the operation to be cancelled if needed, providing better control over long-running tasks.

    // Example usage:
    // using (var unitOfWork = new UnitOfWork())
    // {
    //    //     // Perform multiple operations using repositories
    //   //     await unitOfWork.SaveChangesAsync();
    //   // }
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
