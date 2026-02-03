
using System.Linq.Expressions;

namespace IMS.Application.Abstractions.Persistence
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);

        // void used with update and remove and not Task because these operations are usually synchronous 
        // because they do not talk to the database directly but rather mark the entity as modified or deleted in the context.
        // The actual database operation happens when SaveChangesAsync is called on the DbContext.
        // so making these methods async would not provide any real benefit and could introduce unnecessary complexity.
        void Update(T entity, CancellationToken ct = default);
        void remove(T entity, CancellationToken ct = default);

        // Added AnyAsync method to check for existence of entities matching a predicate
        // This is useful for validation and conditional logic without retrieving full entities
        // example: checking if a user with a specific email already exists before adding a new one

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);



    }
}
