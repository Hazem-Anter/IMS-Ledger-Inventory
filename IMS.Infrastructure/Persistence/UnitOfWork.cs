
using IMS.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Infrastructure.Persistence
{
    // UnitOfWork implementation that manages database transactions
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        // _currentTransaction holds the current active transaction, if any.
        // It is nullable because there may not always be an active transaction.
        private IDbContextTransaction? _currentTransaction; 
        public UnitOfWork(AppDbContext db) 
        { 
            _db = db;
        }

        // HasActiveTransaction checks if there is an active transaction
        // either in the _currentTransaction field or in the DbContext's CurrentTransaction property.
        public bool HasActiveTransaction => 
            _currentTransaction is not null || _db.Database.CurrentTransaction is not null;

        // BeginTransactionAsync starts a new transaction if there isn't already an active one.
        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            // 1) Check if there is already an active transaction.
            // If there is, we do not want to start a new one, so we return early.
            if (HasActiveTransaction) return;

            // 2) If there is no active transaction,
            // we begin a new transaction using the DbContext's Database property.
            _currentTransaction = await _db.Database.BeginTransactionAsync(ct);
        }

        // CommitTransactionAsync commits the current transaction if there is one active.
        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            // 1) Check if there is an active transaction.
            // If there isn't, we cannot commit, so we return early.
            if (_currentTransaction is null) return;

            // 2) If there is an active transaction, we first save any changes made to the DbContext.
            //await _db.SaveChangesAsync(ct);
            // 3) Then we commit the transaction to make all changes permanent in the database.
            await _currentTransaction.CommitAsync(ct);
            // 4) Finally, we dispose of the transaction
            // and set the _currentTransaction field to null to indicate that there is no longer an active transaction.
            await _currentTransaction.DisposeAsync();
            
            _currentTransaction = null;
        }

        // RollbackTransactionAsync rolls back the current transaction if there is one active.
        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            // 1) Check if there is an active transaction.
            // If there isn't, we cannot roll back, so we return early.
            if (_currentTransaction is null) return;

            await _currentTransaction.RollbackAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        // SaveChangesAsync saves all changes made in the context to the database asynchronously.
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
