
using IMS.Application.Abstractions.Persistence;
using IMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IMS.Infrastructure.Repositories
{
    // Generic repository implementation using Entity Framework
    // T is constrained to be a class, ensuring that only reference types can be used with this repository.
    // This class provides basic CRUD operations and can be extended with additional methods as needed.
    // The repository interacts with the AppDbContext to perform database operations.
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        public EfRepository(AppDbContext db) 
        {
            _db = db;
        }

        // Adds a new entity to the database asynchronously, cancelling if requested.
        // T is the type of the entity being added.
        // ct is an optional CancellationToken to cancel the operation. If not provided, the default token is used.
        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _db.Set<T>().AddAsync(entity, ct);

        // Retrieves an entity by its ID asynchronously, cancelling if requested.
        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _db.Set<T>().FindAsync(new object?[] { id }, ct);

        // removes an entity from the database, cancelling if requested.
        public void remove(T entity, CancellationToken ct = default)
            => _db.Set<T>().Remove(entity);

        // Updates an existing entity in the database, cancelling if requested.
        public void Update(T entity, CancellationToken ct = default)
            => _db.Set<T>().Update(entity);

        // Checks if any entity matches the given predicate asynchronously, cancelling if requested.
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => await _db.Set<T>().AnyAsync(predicate, ct);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => await _db.Set<T>().FirstOrDefaultAsync(predicate, ct);

    }
}
