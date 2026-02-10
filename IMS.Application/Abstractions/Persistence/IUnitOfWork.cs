
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


        // Transaction management
        // These methods allow you to explicitly control transactions,
        // which can be useful in scenarios where you need to ensure that a series of operations either all succeed or all fail together.

        // BeginTransactionAsync starts a new transaction. If a transaction is already active,
        // it may throw an exception or create a nested transaction depending on the implementation.
        Task BeginTransactionAsync(CancellationToken ct = default);

        // CommitTransactionAsync commits the current transaction, making all changes permanent in the database.
        Task CommitTransactionAsync(CancellationToken ct = default);

        // RollbackTransactionAsync rolls back the current transaction, undoing all changes made during the transaction.
        Task RollbackTransactionAsync(CancellationToken ct = default);

        // HasActiveTransaction indicates whether there is an active transaction in progress.
        bool HasActiveTransaction { get; }
    }
}
