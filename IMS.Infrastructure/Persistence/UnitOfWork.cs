
using IMS.Application.Abstractions.Persistence;

namespace IMS.Infrastructure.Persistence
{
    // UnitOfWork implementation that manages database transactions
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db) 
        { 
            _db = db;
        }
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
